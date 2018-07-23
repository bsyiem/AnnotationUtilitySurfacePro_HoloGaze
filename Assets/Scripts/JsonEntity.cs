using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonEntity {

    public enum CommandType
    {
        None,
        Send,
        Cancel,
        Annotate
    }

    public CommandType command { get; set; }
    public string annotate { get; set; }

}

