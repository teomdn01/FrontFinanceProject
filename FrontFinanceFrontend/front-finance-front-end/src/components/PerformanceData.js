import { useEffect, useState } from 'react';
import 'react-dropdown/style.css';
import MarketController from '../controller/MarketController';

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export const options = {
  responsive: true,
  plugins: {
    legend: {
      position: 'top',
    },
    title: {
      display: true,
      text: 'Performance Comparison',
    },
  },
};

const PerformanceData = () => {
    let [symbol, setSymbol] = useState('AMZN');
    let [labels, setLabels] = useState([]);
    let [stockDataSet, setStockDataSet] = useState([]);
    let [spyDataSet, setSpyDataSet] = useState([]);
    let [data, setData] = useState();
 
    const getDataPoints = async () => {
      var timeFrame = 'Hourly';
      let e =  document.getElementById('timeFrameSelect');
      if (e.value != '')
        timeFrame = e.value;
      
      getSpyAndSymbolData(timeFrame, setSpyDataSet, setLabels, symbol, setStockDataSet);

    setData( {
      labels: labels,
      datasets: [
        {
          label: symbol,
          data: stockDataSet,
          borderColor: 'rgb(255, 99, 132)',
        },
        {
          label: 'SPY',
          data: spyDataSet,
          borderColor: 'rgb(53, 162, 235)',
        },
      ],
    })
  
};
  
    return (
        <div>
          <div className='stock-inner'>
            <label class="label" for='symbolTxt'>Stock symbol:</label>
            <input type='text' id='symbolTxt' defaultValue={'AMZN'} onChange={e => setSymbol(e.target.value)}></input>
            <label class="label" for='timeFrameSelect'>Data frequency:</label>
            <select id="timeFrameSelect" placeholder='Select an option' defaultValue={'Hourly'}>
              <option value="Hourly">Hourly</option>
              <option value="Daily">Daily</option>
            </select>
            <button className='btn btn-info stock-data' onClick={() => getDataPoints()}>Compare to SPY</button>
          </div>
          <div>
            {data && <Line options={options} data={data} />}
          </div>
        </div>
      );
}
 
export default PerformanceData;

function getSpyAndSymbolData(timeFrame, setSpyDataSet, setLabels, symbol, setStockDataSet) {
  let newDates = [];
  MarketController.getPerformanceData("SPY", timeFrame).then((response) => {
    let stockData = [];
    let newLabels = [];

    response.forEach((elem) => {
      stockData.push(elem.performance - 100);
      let date = new Date(elem.timestamp);
      newLabels.push(date.getUTCMonth() + "/" + date.getUTCDate() + " " + date.getHours() + ":00");
      newDates.push(date.getTime());
    });

    setSpyDataSet(stockData);
    setLabels(newLabels);
    getComparisonData(symbol, timeFrame, newDates, setStockDataSet);
  });
}
function getComparisonData(symbol, timeFrame, newDates, setStockDataSet) {
  MarketController.getPerformanceData(symbol, timeFrame).then((res) => {
    let stockFullData = [];
    let newStockData = [];

    let stockDates = [];
    res.forEach((r) => {
      stockDates.push(new Date(r.timestamp).getTime());
      stockFullData.push(r);
    });

    // If a data point is missing from the stock chart, compute the new value proportionally to its neighbours.
    for (let i = 0; i < newDates.length; ++i) {
      if (stockDates.indexOf(newDates[i]) == -1) {
        for (let j = 0; j < stockFullData.length; ++j) {
          if (new Date(stockFullData[j].timestamp).getTime() > newDates[i]) {
            let left;
            if (j == 0)
              left = stockFullData[j];


            else
              left = stockFullData[j - 1];


            let right = stockFullData[j];
            let newPerformance = (right.performance - left.performance) * (newDates[i] - new Date(left.timestamp).getTime()) / (new Date(right.timestamp).getTime() - new Date(left.timestamp).getTime()) + left.performance;

            stockFullData.splice(j, 0, { timestamp: newDates[i], performance: newPerformance });
            break;
          }
        }
      }
    }

    stockFullData.forEach((d) => newStockData.push(d.performance - 100));

    setStockDataSet(newStockData);
  });
}

