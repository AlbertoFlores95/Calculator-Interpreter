using System;
using System.Collections;
using System.Collections.Generic;

public class Parser {
	Dictionary<string, Object> symbolTable = new Dictionary<string, Object>();
	
	public Object setVariable(String name, Object value){
		if(symbolTable.ContainsKey(name)){
			if((name.Equals("PI"))||(name.Equals("EULER"))){
				Console.WriteLine("Reserved Word: "+ name);
				System.Environment.Exit(1);
			}
			symbolTable[name] = value;
		}
		else
			symbolTable.Add(name, value);
		return value;
	}
	public Object getVariable(String name){
		Object value;
		symbolTable.TryGetValue(name,out value);
		if (value != null) 
			return value;
		return null;
	}

	public Parser() {}
	public Parser(List<Token> tokens){
		this.tokens = tokens;
	}

    public int currentTokenPosition = 0;
    public List<Token> tokens;
	
	public List<Token> getTokens(){
		return tokens;
	}
	
    public Token GetToken(int offset) {
        if (currentTokenPosition + offset >= tokens.Count) {
            return new Token("", "NO_TOKEN");
        }
        return tokens[currentTokenPosition + offset];
    }

    public Token CurrentToken() {
        return GetToken(0);
    }
	
	public Token NextToken() {
        return GetToken(1);
    }

    public void EatToken(int offset) {
        currentTokenPosition = currentTokenPosition + offset;
    }

    public Token MatchAndEat(String type) {
        Token token = CurrentToken();
        if (!CurrentToken().type.Equals(type)) {
			Console.WriteLine("Error: Expecting "+ CurrentToken().type+".Found "+type);
            System.Environment.Exit(1);
        }
        EatToken(1);
        return token;
    }
	
	public Node Assignment(){
		Node node = null;
		String name = MatchAndEat("KEYWORD").text;
		MatchAndEat("ASSIGNMENT");
		Node value = v8Expression();
		node = new AssignmentObject(name, value, this);
		return node;
	}
	
	public Node Statement(){
		Node node = null;
		String type = CurrentToken().type;
		if((type.Equals("KEYWORD"))&&(NextToken().type.Equals("ASSIGNMENT"))){
			node = Assignment();
		}
		else if (type.Equals("PRINT")){
			MatchAndEat("PRINT");
			node = new Print(v8Expression(), "sameline");
		}
		else if (type.Equals("PRINTLN")){
			MatchAndEat("PRINTLN");
			node = new Print(v8Expression(), "newline");
		}
		else{
			//Console.WriteLine("Unknown language construct: "+ CurrentToken().text);
			node = new Print(v8Expression(),"newline");
			//System.Environment.Exit(1);
		}
		return node;
	}
	
	public Node Variable(){
		Token token = MatchAndEat("KEYWORD");
		Node node = new VariableObject(token.text, this);
		return node;
	}
	
	public Node Less(Node v8Expression){
		MatchAndEat("LESS");
		return new Operation("LESS",v8Expression ,ArithmeticExpression());
	}
	
	public Node LessEqual(Node v8Expression){
		MatchAndEat("LESSEQUAL");
		return new Operation("LESSEQUAL",v8Expression ,ArithmeticExpression());
	}
	
	public Node Equal(Node v8Expression){
		MatchAndEat("EQUAL");
		return new Operation("EQUAL",v8Expression ,ArithmeticExpression());
	}
	
	public Node NotEqual(Node v8Expression){
		MatchAndEat("NOTEQUAL");
		return new Operation("NOTEQUAL",v8Expression ,ArithmeticExpression());
	}
	
	public Node Greater(Node v8Expression){
		MatchAndEat("GREATER");
		return new Operation("GREATER",v8Expression ,ArithmeticExpression());
	}
	
	public Node GreaterEqual(Node v8Expression){
		MatchAndEat("GREATEREQUAL");
		return new Operation("GREATEREQUAL",v8Expression ,ArithmeticExpression());
	}

	public Node Power() {
        MatchAndEat("POWER");
        return Factor();
    }
	
	public Node Mod() {
        MatchAndEat("MOD");
        return Factor();
    }
	
    public Node Multiply() {
        MatchAndEat("MULTIPLY");
        return Factor();
    }

    public Node Divide() {
        MatchAndEat("DIVIDE");
        return Factor();
    }

    public Node Add() {
        MatchAndEat("ADD");
        return Term();
    }

    public Node Subtract() {
        MatchAndEat("SUBTRACT");
        return Term();
    }

    public Node Factor() {
        Node result = null;
        if (CurrentToken().type.Equals("LEFT_PAREN")) {
            MatchAndEat("LEFT_PAREN");
            result = v8Expression();
            MatchAndEat("RIGHT_PAREN");
        } else if (CurrentToken().type.Equals("NUMBER")) {
			Token token = MatchAndEat("NUMBER");
			result = new Number(Double.Parse(token.text));
		   //try{Console.WriteLine("factor"+result.opinion());}catch(NullReferenceException){Console.WriteLine("Factor");}
        } else if(CurrentToken().type.Equals("STRING")){
			Token token = MatchAndEat("STRING");
			result = new StringVar(token.text);
		} else if(CurrentToken().type.Equals("KEYWORD")){
			result = Variable();
		}
        return result;
    }
	
	public Node AlmostFactor() {
        Node result = SignedFactor();
        while (CurrentToken().type.Equals("POWER")) {
            switch (CurrentToken().type) {
                case "POWER":
                    result = new Operation("POWER",result,Power());
                    break;
            }
        }
		////try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("AF");}
        return result;
    }
	
	public Node SignedFactor(){
		if (CurrentToken().type.Equals("SUBTRACT")){
			MatchAndEat("SUBTRACT");
			Node result = new NegOp(Factor());
			//try{Console.WriteLine("sub the sign"+result.opinion());}catch(NullReferenceException){Console.WriteLine("SF");}
			return result;
		}
		return Factor();
	}
	
	public Node NotFactor(){
		if (CurrentToken().type.Equals("NOT")){
			MatchAndEat("NOT");
			Node p = Relation();
			return new NotOp(p);
		}
		return Relation();
	}

    public Node Term() {
        Node result = AlmostFactor();
        while (CurrentToken().type.Equals("MULTIPLY") || CurrentToken().type.Equals("DIVIDE")|| CurrentToken().type.Equals("MOD")) {
            switch (CurrentToken().type) {
                case "MULTIPLY":
                    result = new Operation("MULTIPLY",result,Multiply());
                    break;
                case "DIVIDE":
                    result = new Operation("DIVIDE",result,Divide());
                    break;
				case "MOD":
                    result = new Operation("MOD",result,Mod());
                    break;
            }
        }
		//try{Console.WriteLine("term"+result.opinion());}catch(NullReferenceException){Console.WriteLine("Term");}
        return result;
    }

    public Node ArithmeticExpression() {
        Node result = Term();
        while (CurrentToken().type.Equals("ADD") || CurrentToken().type.Equals("SUBTRACT")) {
            switch (CurrentToken().type) {
                case "ADD":
                    result = new Operation("ADD",result,Add());
					//try{Console.WriteLine("add"+result.opinion());}catch(NullReferenceException){Console.WriteLine("AE");}
                    break;
                case "SUBTRACT":
                    result = new Operation("SUBTRACT",result,Subtract());
                   // try{
					//Console.WriteLine("sub"+result.opinion());
					//Console.WriteLine("sub"+Subtract().opinion());
					//}catch(NullReferenceException){Console.WriteLine("sub");}
					break;
            }
        }
        return result;
    }
	
	public Node Relation(){
		Node result = ArithmeticExpression();
		String type = CurrentToken().type;
		//if(type.Equals("EQUAL")||type.Equals("LESS")||type.Equals("GREATER")||type.Equals("LESSEQUAL")||type.Equals("GREATEREQUAL")){
			switch(type){
				case "EQUAL": result = Equal(result);
								//Console.WriteLine(result.ToString());
								break;
				case "NOTEQUAL": result = NotEqual(result);break;
				case "LESS": result = Less(result);break;
				case "GREATER": result = Greater(result);break;
				case "LESSEQUAL": result = LessEqual(result);break;
				case "GREATEREQUAL": result = GreaterEqual(result);break;
			}
		//}
		//try{Console.WriteLine("Rel"+result.opinion());}catch(NullReferenceException){Console.WriteLine("Rela");}
		return result;
	}
	
	public Node v8Factor(){
		return Relation();
	}
	
	public Node v8Term(){
		Node result = NotFactor();
		while(CurrentToken().type.Equals("AND")){
			MatchAndEat("AND");
			result = new Operation("AND",result,NotFactor());
		}
		//try{Console.WriteLine("v8Term"+result.opinion());}catch(NullReferenceException){Console.WriteLine("v8Term");}
		return result;
	}
	
	public Node v8Expression(){
		Node result = v8Term();
		while(CurrentToken().type.Equals("OR")){
			MatchAndEat("OR");
			result = new Operation("OR",result,v8Term());
		}
		//try{Console.WriteLine("v8Expression"+result.opinion());}catch(NullReferenceException){Console.WriteLine("v8Ex");}
		return result;
	}
	
	public void PrettyPrint(List<Token> tokens){
		int numberCount = 0;
		int opCount = 0;
		foreach (Token token in tokens){
			if (token.type == "NUMBER"){
				Console.WriteLine("Number....: " + token.text);
				numberCount++;
			}
			else{
				Console.WriteLine("Operator..: " + token.type);
				opCount++;
			}
		}
		Console.WriteLine("You have got "+numberCount +" different number and " +opCount+" operators.");
	}
	
	public List<Node> Block(){
		List<Node> statements = new List<Node>();
		while ( !(CurrentToken().type.Equals("END"))){
			statements.Add(Statement());
		}
		MatchAndEat("END");
		return statements;
	}
}

public class Interpreter {
        public static void Main(){
            string[] args = {"input.ds"};
			bool debug = false;
            if (args.Length < 1) {
                Console.WriteLine("Usage: Demo <script>");
                return;
                }

            if (args.Length > 1) {
                if (args[1].Equals("debug")) debug = true;
                }
			
            Interpreter interpreter = new Interpreter();
            String sourceCode = System.IO.File.ReadAllText(args[0])+" ";
            interpreter.Interpret(sourceCode, debug);
        }

        public void Interpret(String source, bool debug){
            Tokenizer tokenizer = new Tokenizer();
			
            Parser parser = new Parser(tokenizer.getTokens(source));
			parser.setVariable("PI",3.14159265358979);
			parser.setVariable("EULER",2.718281828459045);
			
            if (debug) DumpTokens(parser);
			parser.MatchAndEat("DS");
            List<Node> script = parser.Block();

			foreach (Node statement in script)
                statement.opinion();
            }

        public void DumpTokens(Parser parser){
			foreach (Token token in parser.getTokens())
                Console.WriteLine("Type: " + token.type + " Text: " + token.text+" ");
				Console.WriteLine();
            }
}

public class Token {
    public String text;
    public String type;
    public Token(String text, String type) {
        this.text = text;
        this.type = type;
    }
}

public class Tokenizer {

    public bool IsOp(char c) {
        return (c=='+') || (c=='-')|| (c=='*') || (c=='/')|| (c=='%')|| (c=='^')
		||(c=='<')||(c=='>')||(c=='=')||(c=='!')||(c=='|')||(c=='&');
    }

    public String FindOpType(char chr,char next) {
        String type = "NONE";
        switch (chr) {
            case '+': type = "ADD"; break;
            case '-': type = "SUBTRACT"; break;
            case '*': type = "MULTIPLY"; break;
            case '/': type = "DIVIDE"; break;
			case '%': type = "MOD"; break;
			case '^': type = "POWER"; break;
			case '<': type = "LESS"; 
					 	if(next=='=') 
							type = "LESSEQUAL";
					 	break;
			case '>': type = "GREATER"; 
					 	if(next=='=') 
							type = "GREATEREQUAL";
					 	break;
			case '=': type = "ASSIGNMENT"; 
					 	if(next=='=') 
							type = "EQUAL";
					 	break;
			case '!': type = "NOT";
					 	if(next=='=') 
							type = "NOTEQUAL";
					 	break;
			case '|': type = "OR";break;
			case '&': type = "AND";break;
        }
        return type;
    }
	
	public String FindParentType(char chr){
		String type = "UNKNOWN";
		switch(chr){
			case '(': type = "LEFT_PAREN"; break;
            case ')': type = "RIGHT_PAREN"; break;
		}
		return type;
	}
	
	public String FindStatementType(String str){
		String type = "UNKNOWN";
		switch(str){
			case "dilansharp": type = "DS";break;
			case "end": type = "END";break;
			case "print":  type = "PRINT";break;
			case "println":type = "PRINTLN";break;
			default:       type = "KEYWORD";break;
		}
		return type;
	}

    public List<Token> getTokens(String source) {
        List<Token> tokens = new List<Token>();
        String tokenString = "";
        String state = "DEFAULT";
		Token token = null;
		char v8 = '\0';
        for (int index = 0; index < source.Length; index++) {
            char chr = source[index];
            switch (state) {
                case "DEFAULT":
                    if (IsOp(chr)) {
						v8 = chr;
						String opType = FindOpType(chr,'\0');
                        //tokens.Add(new Token((""+chr), opType));
						token = new Token((""+chr),opType);
						state = "OPERATOR";
                    } else if (Char.IsDigit(chr)||(chr=='.')) {
                        tokenString += chr;
                        state = "NUMBER";
                    } else if((chr=='(')||(chr==')')){
						String par = FindParentType(chr);
						tokens.Add(new Token((""+chr),par));
					} else if (Char.IsLetter(chr)){
						tokenString += chr;
						state = "KEYWORD";
					} else if(chr == '"'){
						state = "STRING";
					} else if(chr == '$'){
						state = "COMMENT";
					}
                    break;
				case "OPERATOR":
					if(IsOp(chr)){
						String opType = FindOpType(v8,chr);
						token = new Token((""+v8+chr),opType);
					}
					else{
						tokens.Add(token);
						state = "DEFAULT";
						index--;
					}
					break;
                case "NUMBER":
                    if (Char.IsDigit(chr)||(chr=='.')) {
                        tokenString += chr;
                    } else {
                        tokens.Add(new Token(tokenString, "NUMBER"));
                        tokenString = "";
                        state = "DEFAULT";
                        index--;
                    }
                    break;
				case "KEYWORD":
					if (Char.IsLetterOrDigit(chr)){					
						tokenString += chr;					
					}					
					else					{
						String type = FindStatementType(tokenString);					
						tokens.Add(new Token(tokenString, type));					
						tokenString = "";					
						state = "DEFAULT";					
						index--;					
					}					
					break;
				case "STRING":
					if (chr == '"'){
						tokens.Add(new Token(tokenString, "STRING"));
						tokenString = "";
						state = "DEFAULT";
					}
					else{
						tokenString += chr;
					}
					break;
				case "COMMENT":
					if(chr == '\n')
						state = "DEFAULT";
					break;
            }
        }
        return tokens;
    }
}

public abstract class Node{
	public Node(){}
	public abstract Object opinion();
}

public class Number : Node {
    double value;
	
    public Number() {}
	
    public Number(double value) {
        this.value = value;
    }
	
    public override Object opinion() {
		//Console.WriteLine("Number opinion "+value);
        return value;
    }
	
    public String toString() {
        return value + "";
    }
}


public class Boolean : Node {

    bool value;

    public Boolean() {
    }

    public Boolean(bool value) {
        this.value = value;
    }

    public override Object opinion() {
		//Console.WriteLine("Boolean opinion "+value);
        return value;
    }

    public String toString() {
        return value + "";
    }
}

public class StringVar : Node{
	String text;
	public StringVar() {}
	public StringVar(String text){
		this.text = text;
	}
	public override Object opinion(){
		return text;
	}
}


public class Print : Node {

    public Node expression;
    public String type;

    public Print() {
    }

    public Print(Node expression, String type) {
        this.expression = expression;
        this.type = type;
    }

    public override Object opinion() {
        Object writee = expression.opinion();
        if (type.Equals("sameline")) {
            Console.Write(writee);
        } else if (type.Equals("newline")) {
            Console.WriteLine(writee);
        }
        return writee;
    }
}


public class VariableObject : Node {

    public String varName;
    public Parser parser;

    public VariableObject() { }

    public VariableObject(String varName, Parser parser) {
        this.varName = varName;
        this.parser = parser;
    }

    public override Object opinion() {
        Object varValue = parser.getVariable(varName);
        if (varValue == null) {
            Console.WriteLine("Undefined Variable...Var Name: " + varName);
            System.Environment.Exit(1);
        }
        return varValue;
    }
}

public class AssignmentObject : Node{
	public String name;
	public Node value;
	public Parser parser;
	public String scope;
	
	public AssignmentObject() {}
	public AssignmentObject(String name, Node value, Parser parser){
		this.name = name;
		this.value = value;
		this.parser = parser;
	}
	public override Object opinion(){
		return parser.setVariable(name, value.opinion());
	}
}


public class NotOp : Node {

    public Node p;

    public NotOp() { }

    public NotOp(Node p) {
        this.p = p;
    }

    public bool ToBoolean(Node p) {
        Object result = p.opinion();
        return Convert.ToBoolean(result);
    }

    public override Object opinion() {
        Object result = !ToBoolean(p);
		//Console.WriteLine("NotOp opinion "+result);
        return result;
    }
}


public class NegOp : Node {

    public Node p;

    public NegOp() {}

    public NegOp(Node p) {
        this.p = p;
    }

    public double ToDouble(Node p) {
        Object result = p.opinion();
        return Convert.ToDouble(result);
    }

    public override Object opinion() {
        Object result = (-ToDouble(p));
		//Console.WriteLine("NegOp opinion "+result);
        return result;
    }
}

public class Operation : Node {

    public String op;
    public Node left;
    public Node right;

    public Operation() {
    }

    public Operation(String op, Node left, Node right) {
        this.op = op;
        this.left = left;
        this.right = right;
    }

    public double ToDouble(Node p) {
        Object result = p.opinion();
		//Console.WriteLine("To double de operacion"+Double.Parse(result.ToString()));
        return Double.Parse(result.ToString());
    }

    public bool ToBoolean(Node p) {
        Object result = p.opinion();
        return Convert.ToBoolean(result);
    }

    public Object ToObject(Node p) {
        return p.opinion();
    }

    public override Object opinion() {
        Object result = null;
        switch (op) {
            case "ADD":
                result = (Object)(ToDouble(left) + ToDouble(right));
                break;
            case "SUBTRACT":
                result = (Object)(ToDouble(left) - ToDouble(right));
				//Console.WriteLine("Result de subs de oper"+result);
                break;
            case "MULTIPLY":
                result = (Object)(ToDouble(left) * ToDouble(right));
                break;
            case "DIVIDE":
                if (ToDouble(right) == 0) {
                    //Console.WriteLine("Error: Division by Zero!");
                    System.Environment.Exit(1);
                }
                result = (Object)(ToDouble(left) / ToDouble(right));
                break;
			case "MOD": result = (Object)(ToDouble(left)%ToDouble(right));break;
			case "POWER": result = (Object)(Math.Pow(ToDouble(left),ToDouble(right)));break;
            case "LESS":
                result = Convert.ToBoolean(ToDouble(left) < ToDouble(right));
                break;
            case "GREATER":
                result = Convert.ToBoolean(ToDouble(left) > ToDouble(right));
                break;
            case "EQUAL":
                result = Convert.ToBoolean(ToObject(left).Equals(ToObject(right)));
                break;
            case "NOTEQUAL":
                result = Convert.ToBoolean(!ToObject(left).Equals(ToObject(right)));
                break;
            case "LESSEQUAL":
                result = Convert.ToBoolean(ToDouble(left) <= ToDouble(right));
                break;
            case "GREATEREQUAL":
                result = Convert.ToBoolean(ToDouble(left) >= ToDouble(right));
                break;
            case "OR":
                result = Convert.ToBoolean(ToBoolean(left) || ToBoolean(right));
                break;
            case "AND":
                result = Convert.ToBoolean(ToBoolean(left) && ToBoolean(right));
                break;
        }
		//Console.WriteLine("Operation opinion "+result);
        return result;
    }
}
