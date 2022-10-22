import { Component } from "react";

export default class Logout extends Component {
    handleSubmit = e => {
        e.preventDefault();
    };

    render() {
        localStorage.clear();
        return (
            <div>
                <h2>Bye bye</h2>
            </div>
        );
    }
}