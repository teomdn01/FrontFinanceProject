import axios from "axios";

class BrokersAuthController {
     async getAuthToken(data) {
        console.log("Auth data:", data);
        if (localStorage.getItem('token') != null) {
            var response = null;
            try {
                response = await axios.post('/authentication/authenticate', data)
                console.log("getAuthToken", response);
                localStorage.setItem(data.Type + "Token", response.data.accessToken);
             
             } catch (error) {
                 alert(error);
                 return;
             }
             if (response.data.status != 2) {
                alert("Invalid grant!");
             }
             return response;
        }
        else {
            alert('Please log in first');
        }
    }

    async getAuthLink(brokerType){
        if (localStorage.getItem('token') != null) {
            try {
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
                const response = await axios.get('/authentication/authenticate/'+brokerType);
                localStorage.setItem(brokerType + "Token", response.data.accessToken);
                return response;
            } catch (error) {
                console.error(error);
            }
        }
        else {
            return null;
        }
    }
    
    async getBalance(payload){
        if (localStorage.getItem('token') != null) {
            try {
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
                const response = await axios.post('/authentication/balance',
                  payload);
                return response.data;
            } catch (error) {
                console.error(error);
            }
        }
        else {
            return null;
        }
    }

    async getPositions(payload){
        console.log(payload);
        if (localStorage.getItem('token') != null) {
            try {
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
                const response = await axios.post('/authentication/positions', payload);
                return response.data;
            } catch (error) {
                console.error(error);
            }
        }
        else {
            return null;
        }
    }

}
export default new BrokersAuthController();