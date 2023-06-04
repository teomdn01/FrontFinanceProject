import { useEffect, useState } from 'react';
import MarketDataController from '../../controller/MarketDataController';
import "../financial-analysis/FinancialAnalysisComponent.css";

const FinancialAnalysisComponent = () => {
  let [symbol, setSymbol] = useState('');
  let [data, setData] = useState([]);

  useEffect(() => {
  }, [])

  function convertCamelCaseToSentence(camelCaseString) {
    if (!(typeof camelCaseString === 'string' || camelCaseString instanceof String)) {
      return camelCaseString;
    }
    // Convert first character to uppercase
    camelCaseString = camelCaseString.charAt(0).toUpperCase() + camelCaseString.slice(1);
  
    // Insert space before each uppercase letter (except the first character)
    camelCaseString = camelCaseString.replace(/([A-Z])/g, ' $1');
  
    return camelCaseString.trim();
  }

  function convertSentenceToCamelCase(sentenceString) {
    // Remove leading/trailing white spaces
    sentenceString = sentenceString.trim();
  
    // Split the sentence into an array of words
    var words = sentenceString.split(" ");
  
    // Convert the first word to lowercase
    words[0] = words[0].toLowerCase();
  
    // Convert subsequent words to lowercase and capitalize the first letter
    for (var i = 1; i < words.length; i++) {
      words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1).toLowerCase();
    }
  
    // Join the words to form the camelCase string
    var camelCaseString = words.join("");
  
    return camelCaseString;
  }

  async function populateTable(jsonData) {
    // Get the table element
    var table = document.getElementById("analysis-table");

    table.innerHTML = "";

    // Create table headers
    var headerRow = table.insertRow();
    var nameHeader = headerRow.insertCell();
    nameHeader.innerHTML = "Name";
    var valueHeader = headerRow.insertCell();
    valueHeader.innerHTML = "Value";

    var distinctProperties = {};

    // Iterate over the JSON data and populate the table
    for (var key in jsonData) {
      if (jsonData.hasOwnProperty(key)) {
        var value = jsonData[key].value;
        var row = table.insertRow();

        var primaryFieldCell = row.insertCell();
        primaryFieldCell.innerHTML = convertCamelCaseToSentence(key);

        var valueCell = row.insertCell();
        valueCell.innerHTML = value;

        // Iterate over the inner property names and store distinct properties
          for (var prop in jsonData[key]) {
            if (jsonData[key].hasOwnProperty(prop) && prop !== "value" && prop != "toolTip") {
              distinctProperties[prop] = true;
            }
          }
      }
    }

    // Create table headers for distinct inner property names
    for (var prop in distinctProperties) {  
      if (distinctProperties.hasOwnProperty(prop)) {
        var headerCell = headerRow.insertCell();
        headerCell.innerHTML = convertCamelCaseToSentence(prop);

        // Populate the corresponding values in each row for the property
        for (var i = 1; i < table.rows.length; i++) {
          var row = table.rows[i];
          var propName = row.cells[0].innerHTML;
          var propValue = jsonData[convertSentenceToCamelCase(propName)][prop];
          if (propValue == 'toolTip')
            propValue = 'description'
          var valueCell = row.insertCell();
          valueCell.innerHTML = propValue !== undefined ? propValue : "-";
        }
      }
    }

    var headerCell = headerRow.insertCell();
        headerCell.innerHTML = "Description";

        // Populate the corresponding values in each row for the property
        for (var i = 1; i < table.rows.length; i++) {
          var row = table.rows[i];
          var propName = row.cells[0].innerHTML;
          var propValue = jsonData[convertSentenceToCamelCase(propName)]["toolTip"];
          var valueCell = row.insertCell();
          valueCell.innerHTML = propValue !== undefined ? propValue : "-";
        }


    var rows = table.getElementsByTagName("tr");
    for (var i = 1; i < rows.length; i++) {
      var row = rows[i];
    
      // Get the first cell in the row
      var firstCell = row.cells[0];
    
      // Store the tooltip text for the row
      var rowObject = await jsonData[convertSentenceToCamelCase(firstCell.innerHTML)];
      var rowName = await firstCell.innerHTML;
      var tooltipText;
      try { 
        tooltipText = rowObject.toolTip
      } catch (error) {
        console.log(error)
      }    
      // Assign the tooltip text as a custom data attribute
      row.setAttribute("data-tooltip", tooltipText);
    
      // Attach event listener to show/hide the tooltip
      row.addEventListener("mouseover", function() {
        // Get the tooltip text from the custom data attribute
        var tooltipText = this.getAttribute("data-tooltip");
        this.setAttribute("title", tooltipText);
      });
    
      row.addEventListener("mouseout", function() {
        this.removeAttribute("title");
      });
      var score;
      try {
        score = rowObject.value;
        console.log(rowName, score);
      } catch (error) {
        console.log(error)
      }  
  
      if (rowName == "Gross Profit Margin" || rowName == "Operation Margin Ratio" || rowName == "Return On Assets Ratio" 
      || rowName == "Return On Share Holders Equity Ratio" || rowName == "Interest Expense To Operating Income Ratio" ||
      rowName == "Current Assets Liabilities Ratio"
      && score > rowObject.lowerBound) {
        rows[i].classList.add("green-row");
      } else if (rowName == "Net Income To Total Revenue Ratio") {
        if (score > rowObject.upperBound) {
          rows[i].classList.add("green-row");
        }
        else if (score < rowObject.lowerBound) {
          rows[i].classList.add("red-row");
        }
        else {
          rows[i].classList.add("yellow-row");
        }
      } else if (rowName == "Debt To Shareholders Equity Ratio" && score < rowObject.lowerBound) {
        rows[i].classList.add("green-row"); 
      } else if (rowName == "Preferred Stock") {
        if (score == rowObject.desiredValue) {
          rows[i].classList.add("green-row");
        }
        else {
          rows[i].classList.add("yellow-row");
        }
      } else if (rowName == "Long Term Debt") {
          if (score == rowObject.desiredValue) rows[i].classList.add("green-row");
          else rows[i].classList.add("yellow-row");
      } else if (rowName == "Net Of Issuance Stock" && score <= 0) {
        rows[i].classList.add("green-row");
      } else if (rowName == "Investing To Operating Cash Flow Ratio") {
        if (rowObject.desiredValue - rowObject.desiredValue / 2 < score < rowObject.desiredValue + rowObject.desiredValue / 2) {
          rows[i].classList.add("green-row");
        }
        if (score <= rowObject.desiredValue - rowObject.desiredValue || score >= rowObject.desiredValue + rowObject.desiredValue) {
          rows[i].classList.add("red-row");
        }
        else {
          rows[i].classList.add("yellow-row");
        }
      } else if (rowName == "Final Score" && score >= 10) {
        rows[i].classList.add("green-row");
      }
       else {
        rows[i].classList.add("red-row");
      }
    }
  }


    function clearTable() {
      // Get the table element
      var table = document.getElementById("analysis-table");

      // Clear the table content
      table.innerHTML = "";
    }

    async function getFinancialAnalysis(symbol) {
      clearTable();
      var response = await MarketDataController.getFinancialAnalysis(symbol);

      console.log("Financial analysis", response);
      setData(response);
      populateTable(response);
    }


    return (
      <div className='table-analysis'>
        <div>
          <div className='stock-table-input'>
            <label className="label" for='symbolTxt'>Stock symbol:</label>
            <input type='text' id='symbolTxt' placeholder='eg:GOOGL' onChange={e => setSymbol(e.target.value)}></input>
            <button className='btn btn-primary stock-data' onClick={() => getFinancialAnalysis(symbol)}>Run analysis</button>
          </div>
        </div>
        {<div className='table-analysis'>
          <table id='analysis-table' className='table table-striped table-bordered custom-table'>
            <thead className="thead-dark">
            </thead>
            <tbody>
            </tbody>
          </table>
        </div>}
      </div>
    );
  }

  export default FinancialAnalysisComponent;