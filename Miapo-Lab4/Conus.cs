using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Miapo_Lab4
{
    class Conus
    {
            public const string FILENAME = "GeneratedProgram.cs";

            public static bool Generate(List<string> inputParams, List<string> outputParams)
            {
                List<Instruction> instructions = new List<Instruction>();
                List<Instruction> programmInstructions = new List<Instruction>();

                // Сортируем входные и выходные параметры
                inputParams.Sort();
                outputParams.Sort();

                // Получаем нашу "Базу знаний"
                FillKnowledgeBase(ref instructions);

                // Синтезируем программу (прямая волна)
                if (!Synthesize(ref instructions, ref programmInstructions, inputParams, outputParams))
                {
                    return false;
                }

                // Оптимизируем (обратная волна)
                Optimization(ref programmInstructions, ref inputParams, outputParams);

                string sourceCode = GenerateSourceCode(programmInstructions, inputParams, outputParams);

                // Сохраняем код
                StreamWriter file = new StreamWriter(FILENAME);
                file.Write(sourceCode);
                file.Close();

                return true;
            }

            private static void FillKnowledgeBase(ref List<Instruction> instructions)
            {
                
                //Нахождение L
                instructions.Add(new Instruction("L", "L = Math.Sqrt(Math.Pow(r,2.0)+Math.Pow(h,2.0))", new string[] { "r","h" }));
                instructions.Add(new Instruction("L", "L = h/Math.Sin(alfa*Math.PI/180)", new string[] { "h", "alfa" }));
                instructions.Add(new Instruction("L", "L = Sb/(Math.PI*r)", new string[] { "r", "Sb" }));
                instructions.Add(new Instruction("L", "L = Sp/(Math.PI*r)-r", new string[] { "r", "Sp" }));
                //Нахождение h
                instructions.Add(new Instruction("h", "h = Math.Sqrt(Math.Pow(L,2.0)-Math.Pow(r,2.0))", new string[] { "L","r" }));
                instructions.Add(new Instruction("h", "h = L/Math.Sin(alfa*Math.PI/180)", new string[] { "L", "alfa" }));
                instructions.Add(new Instruction("h", "h = 3*V/(Math.PI*Math.Pow(r,2.0))", new string[] { "V", "r" }));
                //Нахождение r
                instructions.Add(new Instruction("r", "r = Math.Sqrt(Math.Pow(L,2.0)-Math.Pow(h,2.0))", new string[] { "h", "L" }));
                instructions.Add(new Instruction("r", "r = Math.Sqrt(So/Math.PI)", new string[] { "So" }));
                instructions.Add(new Instruction("r", "r = Sb/(Math.PI*L)", new string[] { "Sb" ,"L"}));
                instructions.Add(new Instruction("r", "r = Math.PI*(Math.PI*L-4*Sp)/2", new string[] { "Sp", "L" }));
                instructions.Add(new Instruction("r", "r = Math.Sqrt(3*V/(Math.PI*L))", new string[] { "V", "L" }));
                instructions.Add(new Instruction("r", "r = d/2", new string[] { "d"}));
                //Нахождение d
                instructions.Add(new Instruction("d", "d = 2*r", new string[] { "r" }));
                //Нахождение S осн
                instructions.Add(new Instruction("So", "So = Math.PI*Math.Pow(r,2.0)", new string[] { "r" }));
                //Нахождение S бок
                instructions.Add(new Instruction("Sb", "Sb = Math.PI*r*L", new string[] { "r","L" }));
                //Нахождение S полн
                instructions.Add(new Instruction("Sp", "Sp = Math.PI*r*(L+r)", new string[] { "r", "L" }));
                //Нахождение V
                instructions.Add(new Instruction("V", "V =1/3*Math.PI*Math.Pow(r,2.0)*h", new string[] { "r", "h" }));
        }


            // Функция синтезирования исходного текста программы
            // instructions - это вообще все инструкции, а programmInstructions - это те, которые образуются
            private static bool Synthesize(ref List<Instruction> instructions, ref List<Instruction> programmInstructions, List<string> inputParams, List<string> outputParams)
            {
                bool isSuccess = true;

                // Множество переменных, найденных в данный момент
                List<string> foundVars = new List<string>(inputParams);

                // Пока foundVars != outputParams
                while (!Contains(foundVars, outputParams) && isSuccess)
                {
                    bool instructionFound = false;
                    foreach (Instruction instruction in instructions)
                    {
                        if (Contains(foundVars, instruction.Paramethers) && !foundVars.Contains(instruction.Result))
                        {
                            programmInstructions.Add(instruction);
                            foundVars.Add(instruction.Result);

                            instructionFound = true;
                            break;
                        }
                    }
                    // Если инструкцию не нашли - затык. Всё плохо
                    if (!instructionFound)
                    {
                        isSuccess = false;
                    }
                }

                return isSuccess;
            }

            // Оптимизация кода
            private static void Optimization(ref List<Instruction> programmInstructions, ref List<string> inputParams, List<string> outputParams)
            {
                // Полезными переменными является мно-во результирующих переменных
                List<string> usefulVariables = new List<string>(outputParams);

                for (int i = programmInstructions.Count - 1; i >= 0; i--)
                {
                    if (usefulVariables.Contains(programmInstructions[i].Result))
                    {
                        usefulVariables.AddRange(programmInstructions[i].Paramethers);
                        usefulVariables.Remove(programmInstructions[i].Result);
                    }
                    else
                    {
                        programmInstructions.RemoveAt(i);
                    }
                }

                inputParams = usefulVariables;
                inputParams = inputParams.Distinct().ToList();
            }

            // Генерировать исходный код
            private static string GenerateSourceCode(List<Instruction> commands, List<string> inputParams, List<string> outputParams)
            {

                // Создаем словарь переменных
                Dictionary<string, string> internalVars = new Dictionary<string, string>();
                internalVars.Add("L", "double");
                internalVars.Add("h", "double");
                internalVars.Add("r", "double");
                internalVars.Add("So", "double");
                internalVars.Add("Sb", "double");
                internalVars.Add("Sp", "double");
                internalVars.Add("alfa", "double");
                internalVars.Add("d", "double");
                internalVars.Add("V", "double");

                // Код программы
                StringBuilder generatedCode = new StringBuilder();

                // Формируем заголовок
                generatedCode.AppendLine("using System;");
                generatedCode.AppendLine("");
                generatedCode.AppendLine("namespace GeneratedProgram");
                generatedCode.AppendLine("{");
                generatedCode.AppendLine("\tclass Program");
                generatedCode.AppendLine("\t{");
                generatedCode.AppendLine("\t\tpublic static void Main(string[] args)");
                generatedCode.AppendLine("\t\t{");

                // Собираем список всех используемых переменных
                HashSet<string> usedVariables = new HashSet<string>();

                // Добавляем все входные параметры
                foreach (string item in inputParams)
                {
                    usedVariables.Add(item);
                }

                // Добавляем все выходные параметры
                foreach (string item in outputParams)
                {
                    usedVariables.Add(item);
                }

                foreach (Instruction instruction in commands)
                {
                    usedVariables.Add(instruction.Result);
                }

                // Объявляем переменные
                foreach (string variable in usedVariables)
                {
                    if (internalVars.ContainsKey(variable))
                    {
                        // For example: float a;
                        generatedCode.AppendLine("\t\t\t" + internalVars[variable] + " " + variable + ";");
                    }
                }
                generatedCode.AppendLine("\t\t\tstring str = string.Empty;");
                generatedCode.AppendLine();

                // Просим пользователя ввести данные
                foreach (string variable in inputParams)
                {
                    generatedCode.AppendLine("\t\t\tdo {");
                    generatedCode.AppendLine("\t\t\t\tConsole.Write(\"Введите значение " + variable.ToString() + " = \");");
                    generatedCode.AppendLine("\t\t\t\tstr = Console.ReadLine();");
                    generatedCode.AppendLine("\t\t\t}");
                    generatedCode.AppendLine("\t\t\twhile(!double.TryParse(str, out " + variable.ToString() + "));");
                    generatedCode.AppendLine();
                }

                // Вставляем формулы
                foreach (Instruction instruction in commands)
                {
                    generatedCode.AppendLine("\t\t\t" + instruction.formula + ";");
                }

                // Выводим результат
                foreach (string variable in outputParams)
                {
                    generatedCode.AppendLine();
                    generatedCode.AppendLine("\t\t\tConsole.Write(\"Результирующее значение " + variable.ToString() + " = \");");
                    generatedCode.AppendLine("\t\t\tConsole.WriteLine(" + variable.ToString() + ");");
                }

                // Формируем окончание
                generatedCode.AppendLine("\t\t\tConsole.ReadLine();");
                generatedCode.AppendLine("\t\t}");
                generatedCode.AppendLine("\t}");
                generatedCode.AppendLine("}");


                return generatedCode.ToString();
            }

            private static bool Contains(List<string> list1, string[] list2)
            {
                bool isContains = true;

                foreach (String str in list2)
                {
                    isContains = isContains && list1.Contains(str);
                }

                return isContains;
            }

            private static bool Contains(List<string> list1, List<string> list2)
            {
                return Contains(list1, list2.ToArray());
            }
        }
    }

