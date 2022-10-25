import axios from "axios";

class MarketController {
    async getStockData(symbol, timeframe) {
        if (localStorage.getItem('token') != null) {
            try {
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
                const response = await axios.get('/market/' + symbol + '/' + timeframe);
                console.log(response);
                return response.data;
            } catch (error) {
                console.error(error);
            }
        }
        else {
            alert('Please log in first');
        }
    }

    async getPerformanceData(symbol, timeframe) {
        if (localStorage.getItem('token') != null) {
            try {
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
                const response = await axios.get('/market/comparison/' + symbol + '/' + timeframe);
                console.log(response);
                return response.data;
            } catch (error) {
                console.error(error);
            }
        }
        else {
            alert('Please log in first');
        }
    }

}
export default new MarketController();