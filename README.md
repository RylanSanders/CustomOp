# CustomOp

This is a framework that I wrote for automating operations such as web scrapping. 

There is limited documentation in the Documentation folder.
Examples can be found in the Operations.xml folder in CustomOp\bin\Debug\net6.0-windows.

The General structure of the program is centered around the Operations.xml that specifies the different Operations to run.
In this repo, "Processes" refer to a string of operations that ocur on a thread. "Operations" are the actual behavior.
Each Process in the Processes Element in the Operations.xml will have a button to kick off the process on the GUI that is launched when the program is ran.

![alt text](https://github.com/RylanSanders/CustomOp/blob/main/Documentation/ReaMeImages/BasicGUI.PNG?raw=true)

# Examples

The Following process is an example of how the ProcessList Operation Works.
This Process will run the Operated Process(that shows a message box popup) once every second
```xml
<Process name="ListTest">
		<Operation type="SetVar" name="SetVars">
			<Data name="T1" type="string" value="This is a message testing test !" />
		</Operation>
		<Operation type="GenerateIntList" name="ListGenerator">
			<Vars>
				<Data name="StartInt" type = "int" value="0" />
				<Data name="EndInt" type="int" value="10" />
				<Data name="Step" type="int" value="1" />
			</Vars>
		</Operation>
		<Operation type="ProcessList" name="ListProcessor">
			<WaitTime>1000</WaitTime>
			<Vars>
				<Mapping methodName="InputList" varName="IntListOutput" />
			</Vars>
			<OperatedProcess>
				<Process name="SubProcess">
					<Operation type="MessageBox" name="MessageBox">
						<Vars>
							<Data name="Message" type="string" value="This is a message" />
						</Vars>
					</Operation>
				</Process>
			</OperatedProcess>
		</Operation>
	</Process>
```




The follow Process parses out a JSON response for a Magic:The Gathering Decklist from the site <a>https://www.Archidekt.com<a>.
After Parsing the Deck the Process Stores it in a local database
```xml
<Process name="ArkidektScraper">
		<Operation type="ReadFile" name="ReadSQLFile">
			<Vars>
				<Data name="FilePath" type="string" value="DeckList.Json" />
			</Vars>
		</Operation>
		<Operation type="JSONToMap" name="CastJSON">
			<Vars>
				<Mapping methodName="JSonString" varName="ReadText" />
			</Vars>
		</Operation>
		<Operation type="GetMapValue" name="MapToString">
			<Vars>
				<Mapping methodName="MapToRead" varName="DeserializedJSonMap" />
				<Data name="MapKey" type="string" value="cards" />
			</Vars>
		</Operation>
		<Operation type="JSONListToTable" name="MapToString">
			<Vars>
				<Mapping methodName="JSonListString" varName="MapKeyValue" />
			</Vars>
		</Operation>
		<Operation type="DataTableColToList" name="Initial DataTable Creation">
			<Vars>
				<Data name="ColName" type="string" value="card" />
				<Mapping methodName="DataTableToList" varName="JSONDataTable" />
			</Vars>
		</Operation>
		<Operation type="Reduce" name="DataTable Reduction">
			<Accumulator>JSonList</Accumulator>
			<Vars>
				<Mapping methodName="ToReduceList" varName="DataTableColList" />
			</Vars>
		</Operation>
		<Operation type="JSONListToTable" name="Cards Table Maker">
			<Vars>
				<Mapping methodName="JSonListString" varName="ReducedListString" />
			</Vars>
		</Operation>
		<Operation type="DataTableColToList" name="Initial DataTable Creation">
			<Vars>
				<Data name="ColName" type="string" value="oracleCard" />
				<Mapping methodName="DataTableToList" varName="JSONDataTable" />
			</Vars>
		</Operation>
		<Operation type="Reduce" name="DataTable Reduction">
			<Accumulator>JSonList</Accumulator>
			<Vars>
				<Mapping methodName="ToReduceList" varName="DataTableColList" />
			</Vars>
		</Operation>
		<Operation type="JSONListToTable" name="Cards Table Maker">
			<Vars>
				<Mapping methodName="JSonListString" varName="ReducedListString" />
			</Vars>
		</Operation>
		<Operation type="WriteFile" name="WriteFile">
			<Vars>
				<Mapping methodName="TextToWrite" varName="ReducedListString" />
				<Data name="WriteFilePath" type="string" value="intermediateJSON.txt" />
			</Vars>
		</Operation>
		<Operation type="TableToString" name="MapToString">
			<Vars>
				<Mapping methodName="TableToString" varName="JSONDataTable" />
			</Vars>
		</Operation>
		<Operation type="WriteFile" name="WriteFile">
			<Vars>
				<Mapping methodName="TextToWrite" varName="TableString" />
				<Data name="WriteFilePath" type="string" value="FinalJSonCardList.txt" />
			</Vars>
		</Operation>
		<Operation type="StoreTableToDB" name="StoreMap">
			<DataSource>.</DataSource>
			<InitialCatalog>MTG</InitialCatalog>
			<Password>TestUser</Password>
			<User>TestUser</User>
			<DataBase>Sets</DataBase>
			<DBCols DBCol="name" VarName = "name" />
			<Vars>
				<Mapping methodName="TableToDB" varName="JSONDataTable" />
			</Vars>
		</Operation>
	</Process>
