import { useState } from 'react';
import CandleStickChart from './CandleStickChart';
import 'react-dropdown/style.css';
import MarketController from '../controller/MarketController';

const StockData = () => {
    const [charts, setCharts] = useState([]);
    const [symbol, setSymbol] = useState('SPY');
    const [averagePrice, setAveragePrice] = useState(0);
    const [currentPrice, setCurrentPrice] = useState(0);

    const getData = async () => {
      var timeFrame = 'Hourly';
      let e =  document.getElementById('timeFrameSelect');
      if (e.value != '')
        timeFrame = e.value;
      
      const chartList = [];
      const dataPoints = [];
     
      MarketController.getStockData(symbol, timeFrame).then((response) => {
          response.bars.forEach((bar) => {
            dataPoints.push(
              {
                date: new Date(bar.timeUtc),
                open: bar.open,
                high: bar.high,
                low: bar.low,
                close: bar.close,
                volume: bar.volume,
              },
            )
          })

      
        setAveragePrice(response.average);
        setCurrentPrice(response.currentPrice);

      chartList.push(<CandleStickChart key={1} data = {dataPoints} interval={timeFrame}/>)
      setCharts(chartList);

  });
};
  
    return (
        <div>
          <div className='stock-inner'>
            <label class="label" for='symbolTxt'>Stock symbol:</label>
            <input type='text' id='symbolTxt' defaultValue={'SPY'} onChange={e => setSymbol(e.target.value)}></input>
            <label class="label" for='timeFrameSelect'>Data frequency:</label>
            <select id="timeFrameSelect" placeholder='Select an option' defaultValue={'Hourly'}>
              <option value="Hourly">Hourly</option>
              <option value="Daily">Daily</option>
            </select>
            <button className='btn btn-warning stock-data' onClick={() => getData()}>Get data</button>
          </div>
          <div className='stock-text'>
            {averagePrice != 0 && <h2>Average price over the past week: {averagePrice}</h2>}
            {currentPrice != 0 && <h2>Current selling price: {currentPrice}</h2>}
          </div>
          <div className='stock-data'>
            {charts}
          </div>
        </div>
      );
}
 
export default StockData;