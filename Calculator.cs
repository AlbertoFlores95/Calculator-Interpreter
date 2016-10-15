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

	public double Power() {
        MatchAndEat("POWER");
        return Term();
    }
	
	public double Mod() {
        MatchAndEat("MOD");
        return Factor();
    }
	
    public double Multiply() {
        MatchAndEat("MULTIPLY");
        return Factor();
    }

    public double Divide() {
        MatchAndEat("DIVIDE");
        return Factor();
    }

    public double Add() {
        MatchAndEat("ADD");
        return Term();
    }

    public double Subtract() {
        MatchAndEat("SUBTRACT");
        return Term();
    }

    public double Factor() {
        double result = 0;
        if (CurrentToken().type.Equals("LEFT_PAREN")) {
            MatchAndEat("LEFT_PAREN");
            result = ArithmeticExpression();
            MatchAndEat("RIGHT_PAREN");
        } else if (CurrentToken().type.Equals("NUMBER")) {
            result = Double.Parse(CurrentToken().text);
            MatchAndEat("NUMBER");
        }
        return result;
    }
	
	public double AlmostFactor() {
        double result = Factor();
        while (CurrentToken().type.Equals("POWER")) {
            switch (CurrentToken().type) {
                case "POWER":
                    result = Math.Pow(result,Power());
                    break;
            }
        }
        return result;
    }

    public double Term() {
        double result = AlmostFactor();
        while (CurrentToken().type.Equals("MULTIPLY") || CurrentToken().type.Equals("DIVIDE")|| CurrentToken().type.Equals("MOD")) {
            switch (CurrentToken().type) {
                case "MULTIPLY":
                    result = result * Multiply();
                    break;
                case "DIVIDE":
                    result = result / Divide();
                    break;
				case "MOD":
                    result = result % Mod();
                    break;
            }
        }
        return result;
    }

    public double ArithmeticExpression() {
        double result = Term();
        while (CurrentToken().type.Equals("ADD") || CurrentToken().type.Equals("SUBTRACT")) {
            switch (CurrentToken().type) {
                case "ADD":
                    result = result + Add();
                    break;
                case "SUBTRACT":
                    result = result - Subtract();
                    break;
            }
        }
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
		
        Console.WriteLine("Expression Result: " + calc.ArithmeticExpression());
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
        return (c=='+') || (c=='-')|| (c=='*') || (c=='/')|| (c=='%')|| (c=='^')|| (c=='(') || (c==')');
    }

    public String FindOpType(char chr) {
        String type = "NONE";
        switch (chr) {
            case '+': type = "ADD"; break;
            case '-': type = "SUBTRACT"; break;
            case '*': type = "MULTIPLY"; break;
            case '/': type = "DIVIDE"; break;
			case '%': type = "MOD"; break;
			case '^': type = "POWER"; break;
            case '(': type = "LEFT_PAREN"; break;
            case ')':type = "RIGHT_PAREN"; break;
        }
        return type;
    }

    public List<Token> getTokens(String source) {
        List<Token> tokens = new List<Token>();
        String token = "";
        String state = "DEFAULT";
        for (int index = 0; index < source.Length; index++) {
            char chr = source[index];
            switch (state) {
                case "DEFAULT":
                    String opType = FindOpType(chr);
                    if (IsOp(chr)) {
                        tokens.Add(new Token((""+chr), opType));
                    } else if (Char.IsDigit(chr)||(chr=='.')) {
                        token += chr;
                        state = "NUMBER";
                    }
                    break;
                case "NUMBER":
                    if (Char.IsDigit(chr)||(chr=='.')) {
                        token += chr;
                    } else {
                        tokens.Add(new Token(token, "NUMBER"));
                        token = "";
                        state = "DEFAULT";
                        index--;
                    }
                    break;
            }
        }
        return tokens;
    }
}
