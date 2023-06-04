import axios from "axios";

class MarketDataController {
     async getMarketData(payload) {
        if (localStorage.getItem('token') != null) {
            var response = null;
            try {
                response = await axios.post('/marketdata/market-data', payload)
             } catch (error) {
                 alert(error);
                 return;
             }
             if (response.data.status != 0) {
                alert("Invalid grant!");
             }
             return response;
        }
        else {
            alert('Please log in first');
        }
    }

    async getFinancialAnalysis(symbol) {
        if (localStorage.getItem('token') != null) {
            var response = null;
            try {
                response = await axios.get(`/marketdata/finances?symbol=${symbol}`)
             } catch (error) {
                 alert(error);
                 return;
             }
             return response.data;
        }
        else {
            alert('Please log in first');
        }
    }

}
export default new MarketDataController();