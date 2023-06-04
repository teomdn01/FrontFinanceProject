import { useState } from 'react'
import './CredentialsForm.css'
import BrokersAuthController from '../../../controller/BrokersAuthController'

function CredentialsForm(props) {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')

    async function handleLogin(e) {
        e.preventDefault()
        // Code to handle login goes here
        let payload  = {
                "Type": props.brokerType,
                "Username": email,
                "Password": password
        }

        let response = await BrokersAuthController.getAuthToken(payload);
            

        console.log(response);

        props.toggle()
    }

    return (
        <div className="popup">
            <div className="popup-inner">
                <h2>Login</h2>
                <form onSubmit={handleLogin}>
                    <label>
                        Email:
                        <input type="text" value={email} onChange={e => setEmail(e.target.value)} />
                    </label>
                    <label>
                        Password:
                        <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                    </label>
                    <button type="submit">Login</button>
                </form>
                <button onClick={props.toggle}>Close</button>
            </div>
        </div>
    )
}
export default CredentialsForm;