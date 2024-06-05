using System;
using Skyline.DataMiner.Analytics.GenericInterface;
using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.Messages;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;


[GQIMetaData(Name = "ParameterKeyToSeparateColumns")]
public class ParameterKeyToSeparateColumns : IGQIRowOperator, IGQIInputArguments, IGQIColumnOperator
{
    private readonly GQIColumnDropdownArgument _ParameterKeyColumnArg = new GQIColumnDropdownArgument("Parameter Key Column") { IsRequired = true };
    private GQIColumn _parameterKeyColumn;

    private readonly GQIColumn _DMAIDColumn = new GQIIntColumn("DMA ID (int)");
    private readonly GQIColumn _ElementIDColumn = new GQIIntColumn("Element ID (int)");
    private readonly GQIColumn _ParameterIDColumn = new GQIIntColumn("Parameter ID (int)");
    private readonly GQIColumn _TableIndexColumn = new GQIStringColumn("Table Index (String)");


    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _ParameterKeyColumnArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _parameterKeyColumn = args.GetArgumentValue(_ParameterKeyColumnArg);

        return new OnArgumentsProcessedOutputArgs();
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        header.AddColumns(_DMAIDColumn);
        header.AddColumns(_ElementIDColumn);
        header.AddColumns(_ParameterIDColumn);
        header.AddColumns(_TableIndexColumn);
    }

    public void HandleRow(GQIEditableRow row)
    {
        try
        {
            // Convert via ParamID Class
            String parameterKey = row.GetValue<String>(_parameterKeyColumn);
            ParamID paramID = ParamID.FromString(parameterKey);

            row.SetValue(_DMAIDColumn, paramID.DataMinerID);
            row.SetValue(_ElementIDColumn, paramID.EID);
            row.SetValue(_ParameterIDColumn, paramID.PID);
            row.SetValue(_TableIndexColumn, paramID.TableIdx);
        }
        catch (Exception ex) { }
    }
}