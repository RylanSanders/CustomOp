﻿using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    class FileMoveOperation : Operation
    {
        public FileMoveOperation(XElement config) : base(config)
        {
           
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            File.Move(data.getString("SourceFile"), data.getString("DestinationFile"));
        }
    }
}
