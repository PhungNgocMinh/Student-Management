using System;

class Program
{
    struct StudentClass
    {
        public string name;
        public float score;

        public void InputStudentInfo()
        {
            Console.Write("Enter student's name: ");
            this.name = Console.ReadLine();
            Console.Write("Enter student's score: ");
            this.score = Convert.ToSingle(Console.ReadLine());
        }
    }

    static void PrintStudentInfo(StudentClass student)
    {
        Console.WriteLine($"{student.name}\t{student.score}");
    }

    static float FindAverage(StudentClass[] list)
    {
        float sum = 0;
        int numOfStudent = list.Length;
        for (int i = 0; i < numOfStudent; i++)
        {
            sum += list[i].score;
        }
        return sum / numOfStudent;
    }

    static float FindMax(StudentClass[] list)
    {
        float max = list[0].score;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].score > max)
                max = list[i].score;
        }
        return max;
    }

    static int FindNumOfPassStudent(StudentClass[] list)
    {
        int num = 0;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].score >= 5)
                num++;
        }
        return num;
    }

    static void PrintClassInfo(StudentClass[] list)
    {
        Console.WriteLine("---CLASS INFORMATION---");
        foreach (StudentClass item in list)
        {
            PrintStudentInfo(item);
        }
    }

    static void Main()
    {
        Console.Write("Enter number of student: ");
        int numOfStudent = Convert.ToInt32(Console.ReadLine());
        StudentClass[] list = new StudentClass[numOfStudent];

        for (int i = 0; i < numOfStudent; i++)
        {
            Console.WriteLine($"---INFORMATION FOR STUDENT #{i}---");
            list[i].InputStudentInfo();
            Console.WriteLine();
        }

        PrintClassInfo(list);

        Console.WriteLine("---ADDITIONAL VALUES---");
        float average = FindAverage(list);
        float max = FindMax(list);
        int numOfPassStudent = FindNumOfPassStudent(list);
        Console.WriteLine($"Average score: {average}");
        Console.WriteLine($"Maximum score: {max}");
        Console.WriteLine($"Number of passed students: {numOfPassStudent}");
    }
}