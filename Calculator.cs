using System;
using System.Collections;
using System.Collections.Generic;

public class Calculator {

    public int currentTokenPosition = 0;
    public List<Token> tokens;

    public Token GetToken(int offset) {
        if (currentTokenPosition + offset >= tokens.Count) {
            return new Token("", "NO_TOKEN");
        }
        return tokens[currentTokenPosition + offset];
    }

    public Token CurrentToken() {
        return GetToken(0);
    }

    public void EatToken(int offset) {
        currentTokenPosition = currentTokenPosition + offset;
    }

    public Token MatchAndEat(String type) {
        Token token = CurrentToken();
        if (!CurrentToken().type.Equals(type)) {
            System.Environment.Exit(1);
        }
        EatToken(1);
        return token;
    }

	public Protuberance Power() {
        MatchAndEat("POWER");
        return Factor();
    }
	
	public Protuberance Mod() {
        MatchAndEat("MOD");
        return AlmostFactor();
    }
	
    public Protuberance Multiply() {
        MatchAndEat("MULTIPLY");
        return AlmostFactor();
    }

    public Protuberance Divide() {
        MatchAndEat("DIVIDE");
        return AlmostFactor();
    }

    public Protuberance Add() {
        MatchAndEat("ADD");
        return Term();
    }

    public Protuberance Subtract() {
        MatchAndEat("SUBTRACT");
        return Term();
    }

    public Protuberance Factor() {
        Protuberance result = null;
        if (CurrentToken().type.Equals("LEFT_PAREN")) {
            MatchAndEat("LEFT_PAREN");
            result = v8Expression();
            MatchAndEat("RIGHT_PAREN");
        } else if (CurrentToken().type.Equals("NUMBER")) {
           Token token = MatchAndEat("NUMBER");
		   double res=0;
		   try{
		   res = Double.Parse(token.text);
		   }catch(FormatException){Console.WriteLine("Auiiiiii");}
		   result = new Number(res);
		   try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("Factor");}
        }
        return result;
    }
	
	public Protuberance AlmostFactor() {
        Protuberance result = Factor();
        while (CurrentToken().type.Equals("POWER")) {
            switch (CurrentToken().type) {
                case "POWER":
                    result = new Operation("POWER",result,Power());
                    break;
            }
        }
		try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("AF");}
        return result;
    }

    public Protuberance Term() {
        Protuberance result = AlmostFactor();
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
		try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("Term");}
        return result;
    }

    public Protuberance ArithmeticExpression() {
        Protuberance result = Term();
        while (CurrentToken().type.Equals("ADD") || CurrentToken().type.Equals("SUBTRACT")) {
            switch (CurrentToken().type) {
                case "ADD":
                    result = new Operation("ADD",result,Add());
					try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("AE");}
                    break;
                case "SUBTRACT":
                    result = new Operation("SUBTRACT",result,Subtract());
                    break;
            }
        }
        return result;
    }
	
	public Protuberance Relation(){
		Protuberance result = ArithmeticExpression();
		//Protuberance result = null;
		String type = CurrentToken().type;
		//if(type.Equals("EQUAL")||type.Equals("LESS")||type.Equals("GREATER")||type.Equals("LESSEQUAL")||type.Equals("GREATEREQUAL")){
			switch(type){
				case "EQUAL": result = Equal(result);
								//Console.WriteLine(result.ToString());
								break;
				case "LESS": result = Less(result);break;
				case "GREATER": result = Greater(result);break;
				case "LESSEQUAL": result = LessEqual(result);break;
				case "GREATEREQUAL": result = GreaterEqual(result);break;
			}
		//}
		try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("Rela");}
		return result;
	}
	
	public Protuberance Less(Protuberance v8Expression){
		MatchAndEat("LESS");
		return new Operation("LESS",v8Expression ,ArithmeticExpression());
	}
	
	public Protuberance LessEqual(Protuberance v8Expression){
		MatchAndEat("LESSEQUAL");
		return new Operation("LESSEQUAL",v8Expression ,ArithmeticExpression());
	}
	
	public Protuberance Equal(Protuberance v8Expression){
		MatchAndEat("EQUAL");
		return new Operation("EQUAL",v8Expression ,ArithmeticExpression());
	}
	
	public Protuberance Greater(Protuberance v8Expression){
		MatchAndEat("GREATER");
		return new Operation("GREATER",v8Expression ,ArithmeticExpression());
	}
	
	public Protuberance GreaterEqual(Protuberance v8Expression){
		MatchAndEat("GREATEREQUAL");
		return new Operation("GREATEREQUAL",v8Expression ,ArithmeticExpression());
	}
	
	public Protuberance v8Factor(){
		return Relation();
	}
	
	public Protuberance v8Term(){
		Protuberance result = v8Factor();
		while(CurrentToken().type.Equals("AND")){
			MatchAndEat("AND");
			result = new Operation("AND",result,v8Factor());
		}
		try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("v8Term");}
		return result;
	}
	
	public Protuberance v8Expression(){
		Protuberance result = v8Term();
		while(CurrentToken().type.Equals("OR")){
			MatchAndEat("OR");
			result = new Operation("OR",result,v8Term());
		}
		try{Console.WriteLine(result.opinion());}catch(NullReferenceException){Console.WriteLine("v8Ex");}
		return result;
	}

    public static void Main() {
		while(true){
			String expression = "";
			Console.Write("Calculator# ");
			expression = Console.ReadLine();
			expression += " ";
			
			Calculator calc = new Calculator();
			Tokenizer tokenizer = new Tokenizer();
			calc.tokens = tokenizer.getTokens(expression);
			Protuberance result = calc.v8Expression();
			try{Console.WriteLine("Expression Result: " + result.opinion());}catch(NullReferenceException){Console.WriteLine("Main");}
		}
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
			case '=': type = "ASSIGMENT"; 
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
            }
        }
        return tokens;
    }
}

public abstract class Protuberance{
	public Protuberance(){}
	public abstract Object opinion();
}

public class Number : Protuberance {
    double value;
	
    public Number() {}
	
    public Number(double value) {
        this.value = value;
    }
	
    public override Object opinion() {
        return value;
    }
	
    public String toString() {
        return value + "";
    }
}


public class Boolean : Protuberance {

    bool value;

    public Boolean() {
    }

    public Boolean(bool value) {
        this.value = value;
    }

    public override Object opinion() {
        return value;
    }

    public String toString() {
        return value + "";
    }
}

public class Operation : Protuberance {

    public String op;
    public Protuberance left;
    public Protuberance right;

    public Operation() {
    }

    public Operation(String op, Protuberance left, Protuberance right) {
        this.op = op;
        this.left = left;
        this.right = right;
    }

    public double ToDouble(Protuberance p) {
        Object result = p.opinion();
		double r = 0;
		try{
		r = Double.Parse(result.ToString());
		}catch(FormatException){Console.WriteLine("Aqui");}
        return r;
    }

    public bool ToBoolean(Protuberance p) {
        Object result = p.opinion();
        return Convert.ToBoolean(result);
    }

    public Object ToObject(Protuberance p) {
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
                break;
            case "MULTIPLY":
                result = (Object)(ToDouble(left) * ToDouble(right));
                break;
            case "DIVIDE":
                if (ToDouble(right) == 0) {
                    Console.WriteLine("Error: Division by Zero!");
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
        return result;
    }
}
