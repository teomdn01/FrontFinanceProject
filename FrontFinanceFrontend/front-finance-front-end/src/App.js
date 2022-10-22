import './App.css';
import Navbar from './components/navbar/Navbar';
import Home from './Home';
import Login from './components/authentication/Login';
import { Route } from "react-router-dom";
import Signup from './components/authentication/Signup';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.min.js";
import { useEffect, useState } from 'react';
import AuthenticationController from './controller/AuthenticationController';
import Logout from './components/authentication/Logout';

function App() {
  const [user, setUser] = useState();
  useEffect(() => {
    AuthenticationController.getUser().then((response) => {
      setUser(response);
      console.log("GLOBAL USER", response)
    });

  } ,[]);

  return (
    <div className="App">
      <Navbar user={user} />
      <Route path='/home' component={() => <Home user={user} />} />
      <Route path='/login' component={Login} />
      <Route path='/signup' component={Signup} />
      <Route path="/logout" component={Logout} />
    </div>
  );
}

export default App;
