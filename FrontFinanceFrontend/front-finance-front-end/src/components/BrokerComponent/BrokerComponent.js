import { useEffect, useState } from 'react';
import { BrokerTypeEnum, BrokerTypes } from '../../Constants';
import BrokersAuthController from '../../controller/BrokersAuthController';
import MarketDataController from '../../controller/MarketDataController';
import CredentialsForm from '../authentication/CredentialsForm/CredentialsForm';
import '../BrokerComponent/BrokerComponent.css'
import CustomSelect from '../CustomSelect';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';


const BrokerComponent = () => {
  let [symbolsString, setSymbolsString] = useState('AMZN');
  let [balance, setBalance] = useState({});
  let [positions, setPositions] = useState({});
  let [credentialsForm, setCredentialsForm] = useState(false);
  let [tickers, setTickers] = useState([]);
  const [selectedBroker, setSelectedBroker] = useState('');
  const [authBrokers, setAuthBrokers] = useState([]);
  let [renderedComponent, setRenderedComponent] = useState('');

  useEffect(() => {
    updateBrokers();
  }, [balance, positions, tickers]);

  function updateBrokers() {
    BrokerTypes.forEach(broker => {
      if (localStorage.getItem(broker.name + "Token") != undefined && !authBrokers.includes(broker.name)) {
        console.log(broker.name)
        setAuthBrokers((prevList) => [...prevList, broker.name]);
      }

    });
  }
  function togglePop() {
    setCredentialsForm(!credentialsForm);
    updateBrokers();
  };

  // function renderTableData() {
  //   if (tickers != null) {
  //     return tickers.map((o, index) => {
  //       return (
  //         <tr key={index}>
  //           <td>{o.symbol}</td>
  //           <td>{o.marketPrice}</td>
  //           <td>{o.askPrice != 0 ? o.askPrice : "-"}</td>
  //           <td>{o.bidPrice != 0 ? o.bidPrice : "-"}</td>
  //           <td>{o.currency}</td>
  //           <td>{o.lastTradeTimestamp}</td>
  //           <td>{o.brokerType}</td>
  //         </tr>
  //       )
  //     })
  //   }
  // }


  async function getBalance(brokerType) {
    const token = localStorage.getItem(`${brokerType}Token`);
    if (token == null) {
      alert(`Please choose a valid broker or authenticate to ${brokerType} first!`);
      return;
    }

    const payload = {
      "Type": brokerType,
      "AuthToken": token
    }
    let balance = await BrokersAuthController.getBalance(payload);
    setBalance(balance);
    setRenderedComponent('balance')

    console.log("getBalance", balance, positions);
  }

  async function getPositions(brokerType) {
    const token = localStorage.getItem(`${brokerType}Token`);
    if (token == null) {
      alert(`Please choose a valid broker or authenticate to ${brokerType} first!`);
      return;
    }

    const payload = {
      "Type": brokerType,
      "AuthToken": token
    }
    let positions = await BrokersAuthController.getPositions(payload);
    setPositions(positions);
    setRenderedComponent('positions')
    console.log("getPositions", balance, positions);
  }

  async function getBrokerData(brokerType) {
    //console.log("getBrokerData", brokerType);
    if (brokerType == BrokerTypeEnum.Alpaca) {
      var response = await BrokersAuthController.getAuthLink(brokerType);
      console.log("getBrokerData", response);
      window.location.replace(response.data.linkToken)
    }
    else if (brokerType == BrokerTypeEnum.Coinbase) {
      var response = await BrokersAuthController.getAuthLink(brokerType);
      console.log("getBrokerData", response);
      window.location.replace(response.data.linkToken)
    }
    else if (brokerType == BrokerTypeEnum.Trading212) {
      togglePop();
    }
    else {
      alert("Please chose valid broker type");
    }
  }

  // async function getMarketData(brokerType, symbolsString) {
  //   const token = localStorage.getItem(`${brokerType}Token`);
  //   if (token == null) {
  //     alert(`Please choose a valid broker or authenticate to ${brokerType} first!`);
  //     return;
  //   }
  //   const symbols = symbolsString.trim().split(',');
  //   console.log(symbols);
  //   const payload = {
  //     "Type": brokerType,
  //     "AuthToken": token,
  //     "Symbols": symbols
  //   }

  //   var response = await MarketDataController.getMarketData(payload);
  //   var newTickers = [];
  //   response.data.data.forEach(ticker => {
  //     newTickers.push({
  //       symbol: ticker.symbol,
  //       name: ticker.name,
  //       exchange: ticker.exchange,
  //       marketPrice: ticker.marketPrice,
  //       askPrice: ticker.askPrice,
  //       bidPrice: ticker.bidPrice,
  //       currency: ticker.currency,
  //       lastTradeTimestamp: ticker.lastTradeTimestamp,
  //       error: ticker.error,
  //       brokerType: response.data.brokerType
  //     })
  //   })
  //   setTickers(tickers.concat(newTickers).sort((a, b) => a.lastTradeTimestamp < b.lastTradeTimestamp ? 1 : -1));
  //   console.log("Market data", response);
  // }


  // const []

  const handleSelectChange = (value) => {
    setSelectedBroker(value);

  };
  const handleAddClick = async () => {
    if (!authBrokers.includes(selectedBroker)) {
      var response = await getBrokerData(selectedBroker);
      // setAuthBrokers((prevList) => [...prevList, selectedBroker]);
    }
  };


  return (
    <div className="container">
      {credentialsForm ? <CredentialsForm toggle={togglePop} brokerType={selectedBroker} /> : null}
      <div className="sub-container-left">
        <CustomSelect onSelectChange={handleSelectChange}></CustomSelect>
        <div className="sub-container-div-1">
          <div >Connected accounts:</div>
          {authBrokers.map((item, index) => (
            <div className="sub-container-div-3" key={index}>
              <img className='img-logo' src={process.env.PUBLIC_URL + `/resources/${item}Logo.png`} alt="Image" width="40" height="40" />
              <span className='text'>{item}</span>
            </div>
          ))}
        </div>
        <div className="sub-container-div-2">
          <button class="btn btn-outline-success btn-lg btn-transparent" onClick={handleAddClick} style={{ color: '#27296d', padding: '10px 20px;' }}>+ Add another account</button>
        </div>

        <div className="sub-container-div-4">
          <button className='btn btn-action btn-outline-warning btn-lg btn-transparent' style={{ color: '#27296d' }} onClick={() => getBalance(selectedBroker)}>
            <span style={{ textAlign: 'left' }}>View Blances</span>
            <span style={{ textAlign: 'right' }}><FontAwesomeIcon icon={faArrowRight} /></span>
          </button>
        </div>
        <div className="sub-container-div-4">
          <button className='btn btn-action btn-outline-warning btn-lg btn-transparent' style={{ color: '#27296d' }} onClick={() => getPositions(selectedBroker)}>
            <span style={{ textAlign: 'left' }}>View Positions</span>
            <span style={{ textAlign: 'right' }}><FontAwesomeIcon icon={faArrowRight} /></span>
          </button>
        </div>


      </div>
      {/* style={{background: '#00ff00'}} */}
      <div className="sub-container-right" >
        {
          renderedComponent == 'balance' && <div className="sub-container-div-1">
            <h3>Buying power: {balance.buyingPower}$, Cash: {balance.cash}$</h3>
          </div>
        }
        {
          renderedComponent == 'positions' && <div className="sub-container-div-1">
            <h3>Positions: {positions.positions}</h3>
          </div>
        }
      </div>
    </div>
    //   <select className='optionbox' id="brokerSelection" placeholder='Select an option' defaultValue={'-'}></select>
    //   {credentialsForm ? <CredentialsForm toggle={togglePop} brokerType={document.getElementById("brokerSelection").value} /> : null}
    //   {/* <div className='stock-inner'> */}
    //     <div style={{width: '50%', float: 'left'}}>
    //     <CustomSelect></CustomSelect>  
    //     <button className='btn btn-info stock-data' onClick={() => getBrokerData(document.getElementById("brokerSelection").value)}> Authenticate with broker</button>

    //     <button className='btn btn-primary stock-data' onClick={() => getPortfolio(document.getElementById("brokerSelection").value)}> View Blances & Positions</button>
    //     </div>



    //     <div style={{width: '50%', float: 'right'}}>
    //     {
    //       balance &&
    //       <section className='portfolio-box'>
    //         <div>
    //           <h2>Buying power: {balance.buyingPower}$, Cash: {balance.cash}$</h2>
    //         </div>
    //         <div>
    //           <h3>Positions: {positions.positions}</h3>
    //         </div>
    //       </section>
    //     }
    //     </div>

    //     <div className='stock-table-input'>
    //       <label class="label" for='symbolTxt'>Stock symbol:</label>
    //       <input type='text' id='symbolTxt' placeholder='eg:TSLA,META' onChange={e => setSymbolsString(e.target.value)}></input>
    //       <button className='btn btn-warning stock-data' onClick={() => getMarketData(document.getElementById("brokerSelection").value, symbolsString)}>View Price</button>
    //     </div>
    //     {tickers.length > 0 && <div className='optionals'>
    //       <table className='table table-striped table-bordered table-sm'>
    //         <thead>
    //           <tr>
    //             <th>Symbol</th>
    //             <th>Market Quote</th>
    //             <th>Ask Price</th>
    //             <th>Bid price</th>
    //             <th>Currency</th>
    //             <th>Last Trade</th>
    //             <th>Broker Type</th>
    //           </tr>
    //         </thead>
    //         <tbody>
    //           {renderTableData()}
    //         </tbody>
    //       </table>
    //       <h2>{ }</h2>
    //     </div>}

    // </div>
  );
}

export default BrokerComponent;