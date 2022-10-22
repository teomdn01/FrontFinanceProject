import { useState } from "react";
import AuthenticationController from "../../controller/AuthenticationController";
const Signup = () => {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [firstName, setFirstName] = useState();
    const [lastName, setLastName] = useState();
    
    const handleSubmit = e => {
        e.preventDefault();
        const data = {
            firstName: firstName,
            lastName: lastName,
            email: email,
            password: password,

        };

        AuthenticationController.signup(data);
    };

    return ( 
        <div className="auth-wrapper">
            <div className="auth-inner">
                <form onSubmit={handleSubmit}>
                    <h3>Sign Up</h3>
                    <div className="form-group">
                        <label>First Name</label>
                        <input type="text" className="form-control" placeholder="First Name"
                            onChange={e => setFirstName(e.target.value)} />
                    </div>

                    <div className="form-group">
                        <label>Last Name</label>
                        <input type="text" className="form-control" placeholder="Last Name"
                            onChange={e => setLastName(e.target.value)} />
                    </div>

                    <div className="form-group">
                        <label>Email</label>
                        <input type="email" className="form-control" placeholder="Email"
                            onChange={e => setEmail(e.target.value)} />
                    </div>

                    <div className="form-group">
                        <label>Password</label>
                        <input type="password" className="form-control" placeholder="Password"
                            onChange={e => setPassword(e.target.value)} />
                    </div>
                    <button className="btn btn-primary btn-block submit-btn">Sign Up</button>

                </form>
            </div>
        </div>
     );
}
 
export default Signup;
