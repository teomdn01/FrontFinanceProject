import { Link } from 'react-router-dom';
import Login from './components/authentication/Login';
const Home = (props) => {
    if (props.user) {
        return (
            <div>
                <h1>Hello, {props.user.firstName}. Welcome to our amazing platform!!</h1>
            </div>
        )   
    }
    else {
        return (
            <div>
                <div className="home-login-div">
                    <Login className="home-login"></Login>
                </div>
                <div className='home-sign-up-div'>
                    <h3>Not registered?</h3> 
                    <Link to="/signup" className="btn btn-primary login-button">Sign-Up</Link>
                </div>
            </div>
        )   
    }
}
 
export default Home;