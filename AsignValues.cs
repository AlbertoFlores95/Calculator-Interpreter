using System;
using System.Collections.Generic;
using System.Collections;
public class AsignValues{
	Dictionary<string, string> dictionary = new Dictionary<string, string>();
	public static void Main(){
		new AsignValues();
	}
	
	AsignValues(){
		string sourceCode="";
		string[] lines = System.IO.File.ReadAllLines("input.ds");
		
		foreach (string line in lines){
            sourceCode = line;
			if(sourceCode.IndexOf("var ")==0)
				saveValue(sourceCode);
			else if(sourceCode.IndexOf("print:")==0){
				print(sourceCode);
			}
			else if(sourceCode.IndexOf("println:")==0){
				println(sourceCode);
			}
			else if(sourceCode.IndexOf("PI*")==0){
				Console.Write(""+3.14159265358979);
			}
			else if(sourceCode.IndexOf("charizard")==0){
				charizard();
			}
			else if(sourceCode.IndexOf("calc:")==0){
				goToCalculator(sourceCode);
			}
			else
				printLine(sourceCode);
        }
		
		/*Console.Write("Escribe la variable que quieres: ");
		String var = Console.ReadLine();
		dictionary.TryGetValue(var,out var);
		Console.Write("Su valor es "+var);*/
	}
	
	void saveValue(string sourceCode){
		int intVar = sourceCode.IndexOf("var");//Console.Write("Su valor es "+intVar);
		int intEqual = sourceCode.IndexOf("=");
		string variable ="";
		string value = "";
		bool IsString = false;
		int counter = 0;
		
		foreach(char c in sourceCode.Substring(intVar+3,sourceCode.Length-intVar-3)){//Sacar la variable
			if(c=='=')
				break;
			if(c!=' ')
				variable+=c;
		}
		
		foreach(char c in sourceCode.Substring(intEqual,sourceCode.Length-intEqual)){//Sacar el valor
			if(c=='"'){//Encuentra las comillas y las cuenta
				IsString = true;
				counter++;
			}
			if((c!=' ')&&(IsString==false))//No es string, siempre usa este
				value+=c;
			if(IsString&&(c!='"'))
				value+=c;
			if(counter>=2)//Si son 2 comillas
				break;
			
		}
		value = value.Substring(1,value.Length-1);
		dictionary.Add(variable, value);
	}
	
	void print(string sourceCode){
		int intPrint = sourceCode.IndexOf("print:");
		Console.Write(""+sourceCode.Substring(intPrint+6,sourceCode.Length-intPrint-6));
	}
	
	void println(string sourceCode){
		int intPrint = sourceCode.IndexOf("println:");
		Console.WriteLine(""+sourceCode.Substring(intPrint+8,sourceCode.Length-intPrint-8));
	}
	
	void printLine(string sourceCode){
		string var = sourceCode;
		dictionary.TryGetValue(var,out var);
		Console.Write(var);
	}
	
	void goToCalculator(String expression) {
			/*Console.Write("Calculator# "); expression = Console.ReadLine();*/
			Calculator calc = new Calculator();
			Tokenizer tokenizer = new Tokenizer();
			int intCalc = expression.IndexOf("calc:");
			expression = expression.Substring(intCalc+5,expression.Length-intCalc-5);
			expression += " ";
			calc.tokens = tokenizer.getTokens(expression);
			Protuberance result = calc.v8Expression();
			try{Console.WriteLine("Expression Result: " + result.opinion());}catch(NullReferenceException){Console.WriteLine("Main");}
    }
	
	void charizard(){                                                                                  
Console.WriteLine("                                                                                        ");
Console.WriteLine("                          `                                                             ");
Console.WriteLine("                          ,`                                  `.                        ");
Console.WriteLine("                         ;.`                                  ;,`                       ");
Console.WriteLine("                       `;,+                                    `:                       ");
Console.WriteLine("                      ,+::        #:,      `                   ;:'                      ");
Console.WriteLine("                    :`,,:+`     +.;:,.;:;;                     +';:.                    ");
Console.WriteLine("                  ,`::,:::      ;::,,::,#+''                  ;,';:.                    ");
Console.WriteLine("           `    ;.,,;+'+::      ';+,.+,::;,:+                 .:+':,;` `                ");
Console.WriteLine("             ',`.,+++''++,      #,;,:::';;;';'             ` ;;++++::+                  ");
Console.WriteLine("            `.,,''+'++'++:'     `;,::;;;;';;;;`.             .,'''''':'                 ");
Console.WriteLine("        ```::,:+''++++'+'::``    ,:;'+;;;;;;;:;:+          `.,#'++'+'+,:                ");
Console.WriteLine("          ':::''''++'+'+++:`       ,;:;;;;;;;`:+;+`         ;,'+'+'++''::'              ");
Console.WriteLine("         '::'''++'+''++''+:+        `+;;;;;;;.  .`          .''+++''''++::.;            ");
Console.WriteLine("       `';,+''++''''''+''+,;          ';:';';+             ',+'++'+++'+'#,::`           ");
Console.WriteLine("       :::''++''''''+'+'+':,`           ;'+';:             .,++'++++++'++'+,,,          ");
Console.WriteLine("   ` `'::'+''+''''''++'+'+';'           ';;;:,            .`'+''++'''''''+++;:,         ");
Console.WriteLine("     ',,''++'''''''''+++'++,,           ;;;;::.           ;,+''+''''''''''''',,         ");
Console.WriteLine("   `',,+''+'''''''''+'+'++#,:+`         `:;:::+           .:+++++''''''''''''#:;        ");
Console.WriteLine("    ::+'++'''''''''++''+'++:::,:         :::::;          +:,+''++'+'''''''''+'::        ");
Console.WriteLine("    :,''+'''''''''''++'+'''++':;.;       :::::,        +`:;;+++++'''''''''''+'#:,       ");
Console.WriteLine("   ':+''+''''''''''++++''''''+++,:,,     ::::::       :,::,'+''++''''''''''''+'::       ");
Console.WriteLine("   ::''''''''''''''''+'++''++++'';::'    ::::::,    :`:,;++'+'''++'''''''''''+'+:``     ");
Console.WriteLine("  `:#'+''''''''''''++''+'''++'++''+,,':. ::::::'`.;;`,+'''+++''''+''''''''''''+','      ");
Console.WriteLine("  ':''+''''''+'+''''++''''''+++++''',::, ::::::'`,..,+''''+++''+'+''''''''''''''#,      ");
Console.WriteLine("  ;+'''''''''''''''++'+'''''''+'''''+:::;:::::,,#:::#'++'+'+'''++'''''''''''''''+:;     ");
Console.WriteLine("  ,++'''''''#: .'''+++''''''''''++'+++,,::;::;;,,':++''+++'''''++''''#''''''''''+:;     ");
Console.WriteLine(" ;:++''''':     `''''+''''''''''''''#,:':::::::::,,:;'''+'''';'++''':  ''+'''''''#,,    ");
Console.WriteLine(" ;+''''+: `      +'''+'''''''+''''':;:,::::::::::,,:::;+#+#+';'++'''    `+;'''''++:'    ");
Console.WriteLine(" :+''''           ''+'''''''#::::::::,::::::::::::::::::::;.+''+''#    :;.`#'''+''::    ");
Console.WriteLine(",,+';`            ''+'+';+;':::,::::::::::::::::::::;:::::::,;'+'#     :;., ,''+''::    ");
Console.WriteLine("',''.` `           '+''#   ':::'''+++'':::::::::::::+++'++:;;:`+;      :;;;   #'''::    ");
Console.WriteLine(";:+#               ++'+   '::::++++##;::,,:,,+'+;,::,+@`+'',::::'``   `:;;,    ++'::    ");
Console.WriteLine("::'                `+#   +::::;'# `@,::::;:+`.....;:,#`  '':::::::'    ;;;,.   +'+:;    ");
Console.WriteLine(":'#                 `  +,::::;'+   +,::::,:........+:'    ;`+;:::::'   ,;:,,   .++:'    ");
Console.WriteLine(":+`                   +::::::'+    ,:::::;..........#:   `   '::::':;`.:;.,.    '#:     ");
Console.WriteLine("+#                   .::;::::,    ,;::::#............:,      +;,::++#`;:,,,     ++'`    ");
Console.WriteLine(" +                   `:@':;,:'    '::::,.............:+      ;:: ,:: `;::,,`,   +;:     ");
Console.WriteLine("                      :;,:`;::    ,::::+.............`;      ':: ,+` ``;;.,,;`  .,      ");
Console.WriteLine("                       ::+`:,:   `,::::`..............'      ` ' `   `:,:.,;;'   '      ");
Console.WriteLine("                      ` ,;`',`   .::::+................:              ;.,,,,.'  ::      ");
Console.WriteLine("                      `;;,:';.  ``::::'...............`:,             ;,,,,,    ` `     ");
Console.WriteLine("                     ,`,::,.`. ``.::::;................::``          `;,,,,`            ");
Console.WriteLine("                    ;,::,,:,;',``:::::;..............:,;:.`;`          `:,              ");
Console.WriteLine("                   :::,;;;:,,+.`::::::;,...........,,,';:;,.:          ;' `             ");
Console.WriteLine("                  .:::::::::;..::::::,;,........,,,,:,;;:::...        ,+`               ");
Console.WriteLine("                 `.::::::::,:,:::::::;;,:,....,,,,,,:;;;;:::..,     ','+                ");
Console.WriteLine("                 :::::::::::,:::::::;;:,,,,:,:,,:,,:,;;;;;:::.;  ;:.:+'                 ");
Console.WriteLine("                `::::::::::+::::::;;;;+,,,,,,:,,,:,.;;;;;;;:::``,:::++                  ");
Console.WriteLine("                `:::::::::;':::::;;;;:;,:::,,:,:::,;;;;;;;;;::,:,;'.'                   ");
Console.WriteLine("                 ;;;::;;;;+:::::;;;;;#;.,,,,,,,:,+::;;;;;;;;::+',:;`                    ");
Console.WriteLine("                 .;+;++',,,;::;;';;:',:,'+::;'+:,,,,+;;;;;;;;',.'.                      ");
Console.WriteLine("                  :.,,,,,:,';;;;;';,,,,.;+.     ,''..:'';;;',+: `                       ");
Console.WriteLine("                   `:,:,,:.;;;;;;'':.                ;;;;;;',`                          ");
Console.WriteLine("                   ``;''';:,:'+;;;,                `';;;;';,:':`                        ");
Console.WriteLine("                    `     ;:::;;';,                  ;;;;;;,,:;'                        ");
Console.WriteLine("                        ``+ :,;,,;                   `,+;;;:;. ;+                       ");
Console.WriteLine("                          ``;`;:,;                       ;'+.;++                        ");
Console.WriteLine("                          ' .`` ''                          ,,                          ");
Console.WriteLine("                        `    :                                                          ");
Console.WriteLine("                                                                                        ");
Console.WriteLine("                                                                                        ");
Console.WriteLine("                                                                                        ");
Console.WriteLine("                                                                                        ");
Console.WriteLine("                                                                                        ");
Console.WriteLine("                                                                                        ");
Console.WriteLine("                                                                                        ");
	}
}




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

	public Protuberance Power() {
        MatchAndEat("POWER");
        return Factor();
    }
	
	public Protuberance Mod() {
        MatchAndEat("MOD");
        return Factor();
    }
	
    public Protuberance Multiply() {
        MatchAndEat("MULTIPLY");
        return Factor();
    }

    public Protuberance Divide() {
        MatchAndEat("DIVIDE");
        return Factor();
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
		   result = new Number(Double.Parse(token.text));
		   //try{Console.WriteLine("factor"+result.opinion());}catch(NullReferenceException){Console.WriteLine("Factor");}
        }
        return result;
    }
	
	public Protuberance AlmostFactor() {
        Protuberance result = SignedFactor();
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
	
	public Protuberance SignedFactor(){
		if (CurrentToken().type.Equals("SUBTRACT")){
			MatchAndEat("SUBTRACT");
			Protuberance result = new NegOp(Factor());
			//try{Console.WriteLine("sub the sign"+result.opinion());}catch(NullReferenceException){Console.WriteLine("SF");}
			return result;
		}
		return Factor();
	}
	
	public Protuberance NotFactor(){
		if (CurrentToken().type.Equals("NOT")){
			MatchAndEat("NOT");
			Protuberance p = Relation();
			return new NotOp(p);
		}
		return Relation();
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
		//try{Console.WriteLine("term"+result.opinion());}catch(NullReferenceException){Console.WriteLine("Term");}
        return result;
    }

    public Protuberance ArithmeticExpression() {
        Protuberance result = Term();
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
	
	public Protuberance Relation(){
		Protuberance result = ArithmeticExpression();
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
		//try{Console.WriteLine("Rel"+result.opinion());}catch(NullReferenceException){Console.WriteLine("Rela");}
		return result;
	}
	
	public Protuberance v8Factor(){
		return Relation();
	}
	
	public Protuberance v8Term(){
		Protuberance result = NotFactor();
		while(CurrentToken().type.Equals("AND")){
			MatchAndEat("AND");
			result = new Operation("AND",result,NotFactor());
		}
		//try{Console.WriteLine("v8Term"+result.opinion());}catch(NullReferenceException){Console.WriteLine("v8Term");}
		return result;
	}
	
	public Protuberance v8Expression(){
		Protuberance result = v8Term();
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
		Console.WriteLine("You have got "+
		numberCount +
		" different number and " +
		opCount
		+" operators.");
		
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
		//Console.WriteLine("Number opinion "+value);
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
		//Console.WriteLine("Boolean opinion "+value);
        return value;
    }

    public String toString() {
        return value + "";
    }
}

/*public class Variable : Protuberance {

    public String varName;
    public Parser parser;

    public Variable() { }

    public Variable(String varName, Parser parser) {
        this.varName = varName;
        this.parser = parser;
    }

    public Object opinion() {
        Object varValue = parser.getVariable(varName);
        if (varValue == null) {
            Util.Writeln("Undefined Variable...Var Name: " + varName);
            System.exit(1);
        }
        return varValue;
    }
}*/


public class NotOp : Protuberance {

    public Protuberance p;

    public NotOp() { }

    public NotOp(Protuberance p) {
        this.p = p;
    }

    public bool ToBoolean(Protuberance p) {
        Object result = p.opinion();
        return Convert.ToBoolean(result);
    }

    public override Object opinion() {
        Object result = !ToBoolean(p);
		//Console.WriteLine("NotOp opinion "+result);
        return result;
    }
}


public class NegOp : Protuberance {

    public Protuberance p;

    public NegOp() {}

    public NegOp(Protuberance p) {
        this.p = p;
    }

    public double ToDouble(Protuberance p) {
        Object result = p.opinion();
        return Convert.ToDouble(result);
    }

    public override Object opinion() {
        Object result = (-ToDouble(p));
		//Console.WriteLine("NegOp opinion "+result);
        return result;
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
		//Console.WriteLine("To double de operacion"+Double.Parse(result.ToString()));
        return Double.Parse(result.ToString());
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
