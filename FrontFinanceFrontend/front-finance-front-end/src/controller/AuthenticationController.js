import axios from "axios";

class AuthenticationController {
     async signup(data) {
        console.log("Signup data:", data);
         try {
            const response = await axios.post('/authentication/register', data)
         } catch (error) {
             console.log("ERROR", error);
             alert(error.response.data.errors);
             return;
         }
         alert("Success. Log in!");
    }

    async getUser(){
        if (localStorage.getItem('token') != null) {
            try {
                axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
                const response = await axios.get('/authentication/get-authenticated-user');
                //console.log(response);
                return response.data;
            } catch (error) {
                console.error(error);
            }
        }
        else {
            return null;
        }
    }

    async login(data) {
        let user = "";
        try {
            const response = await axios.post('/authentication/login', data);
            localStorage.setItem('token', response.data.token);
            console.log("RESPONSE", response.data);
            user = response.data;
        } catch(error) {
            console.log(error);
            user = "bad-credentials";
        }
        
        return user;
    }

}
export default new AuthenticationController();