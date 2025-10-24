// See https://aka.ms/new-console-template for more information
using 无功功率补偿;

Console.WriteLine("Hello, World!");


Capacitancecompensation capacitancecompensation = new Capacitancecompensation();

//计算1 有功功率
float resultyggl = capacitancecompensation.GetActivePower(300f, 0.85f);
Console.WriteLine(resultyggl);


//计算2 无功功率
Console.WriteLine(capacitancecompensation.GetReactivePower(300f, 0.85f).ToString());

//计算3 相角
Console.WriteLine(capacitancecompensation.GetTargetPhaseAngle(0.99f));

//计算4 目标无功功率
Console.WriteLine(capacitancecompensation.GetTargetWugonglv(capacitancecompensation.GetActivePower(300f, 0.85f), 0.99f));


//计算5 补偿无功容量
Console.WriteLine(capacitancecompensation.GetCompensateReactivePower(
    capacitancecompensation.GetReactivePower(300f, 0.85f),
    capacitancecompensation.GetTargetWugonglv(resultyggl, 0.99f)
    ));