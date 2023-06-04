import { useState } from "react";
import { MenuItems, MenuItemsLoggedIn } from "../../Constants";
import './Navbar.css'

const Navbar = (props) => {
    const [clicked, setClicked] = useState(0);

    function handleClick() {
        setClicked(!clicked);
    }
    console.log(props.user)

    let buttons;
    if (props.user) {
        buttons = (
            <ul className={clicked ? 'nav-menu active' : 'nav-menu'}>
                {MenuItemsLoggedIn.map((item, index) => {
                    return (
                        <li key={index}>
                            <a className={item.cName} href={item.url} >
                                {item.title}
                            </a>
                        </li>
                    )
                })}
            </ul>
        )
    }
    else {
        buttons = (
            <ul className={clicked ? 'nav-menu active' : 'nav-menu'}>
                {MenuItems.map((item, index) => {
                    return (
                        <li key={index}>
                            <a className={item.cName} href={item.url} >
                                {item.title}
                            </a>
                        </li>
                    )
                })}
            </ul>
        )

    }

    return (
        <nav className="navbar-items">
            <h1 className="navbar-logo">Investments 101<i className="fab fa-react"></i></h1>
            <div className="menu-icon" onClick={handleClick}>
                <i className={clicked ? 'fas fa-times' : 'fas fa-bars'}></i>
            </div>
            {buttons}
        </nav>
    )
}
 
export default Navbar;