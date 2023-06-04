import React, { useEffect } from 'react'
import { useHistory, useLocation } from 'react-router-dom/cjs/react-router-dom';
import BrokersAuthController from '../../controller/BrokersAuthController';

function OAuth2Handler(props) {
    const search = useLocation().search;
    const code = new URLSearchParams(search).get('code');    
    const history = useHistory();  
    
    useEffect(() => {
      async function auth() {
        let brokerType = props.match.params.broker.charAt(0).toUpperCase() + props.match.params.broker.slice(1); //making first letter uppercase
        var response = await BrokersAuthController.getAuthToken({
          "AuthToken": code,
          "Type": brokerType
      })
        console.log("Use effect auth", response);
      }

      auth();
      history.push("/brokers");
    }, [])
  
  return (
    <div>
        <h2>Authorizing {props.match.params.broker}...</h2>
    </div>
  )
}

OAuth2Handler.propTypes = {}

export default OAuth2Handler
