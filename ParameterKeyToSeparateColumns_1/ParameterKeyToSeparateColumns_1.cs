using System;
using Skyline.DataMiner.Analytics.GenericInterface;
using Skyline.DataMiner.Net;

[GQIMetaData(Name = "ParameterKeyToSeparateColumns")]
public class ParameterKeyToSeparateColumns : IGQIRowOperator, IGQIInputArguments, IGQIColumnOperator
{
    private readonly GQIColumnDropdownArgument _parameterKeyColumnArg = new GQIColumnDropdownArgument("Parameter Key Column") { IsRequired = true };
    private readonly GQIColumn _dMAIDColumn = new GQIIntColumn("DMA ID (int)");
    private readonly GQIColumn _elementIDColumn = new GQIIntColumn("Element ID (int)");
    private readonly GQIColumn _parameterIDColumn = new GQIIntColumn("Parameter ID (int)");
    private readonly GQIColumn _tableIndexColumn = new GQIStringColumn("Table Index (String)");

    private GQIColumn _parameterKeyColumn;

    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _parameterKeyColumnArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _parameterKeyColumn = args.GetArgumentValue(_parameterKeyColumnArg);

        return new OnArgumentsProcessedOutputArgs();
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        header.AddColumns(_dMAIDColumn);
        header.AddColumns(_elementIDColumn);
        header.AddColumns(_parameterIDColumn);
        header.AddColumns(_tableIndexColumn);
    }

    public void HandleRow(GQIEditableRow row)
    {
        // Convert via ParamID Class
        String parameterKey = row.GetValue<String>(_parameterKeyColumn);
        ParamID paramID = ParamID.FromString(parameterKey);

        row.SetValue(_dMAIDColumn, paramID.DataMinerID);
        row.SetValue(_elementIDColumn, paramID.EID);
        row.SetValue(_parameterIDColumn, paramID.PID);
        row.SetValue(_tableIndexColumn, paramID.TableIdx);
    }
}