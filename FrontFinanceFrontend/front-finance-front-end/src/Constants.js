
export const MenuItems = [
    {
        title: 'Home',
        url: '/home',
        cName: 'nav-links'
    },
    {
        title: 'Login',
        url: '/login',
        cName: 'nav-links'
    },
    {
        title: 'Signup',
        url: '/signup',
        cName: 'nav-links'
    }
]

export const MenuItemsLoggedIn = [
    {
        title: 'Home',
        url: '/home',
        cName: 'nav-links'
    },
    {
        title: 'Stock History',
        url: '/stock-data',
        cName: 'nav-links'
    },
    {
        title: 'Performance Comparison',
        url: '/performance',
        cName: 'nav-links'
    },
    {
        title: 'Brokers Data',
        url: '/brokers',
        cName: 'nav-links'
    },
    {
        title: 'Financial Analysis',
        url: '/analysis',
        cName: 'nav-links'
    },
    {
        title: 'Logout',
        url: '/logout',
        cName: 'nav-links'
    }
]

export const BrokerTypes = [
    {
        name: "Alpaca"
    },
    {
        name: "Freedom"
    },
    {
        name: "Tradier"
    },
    {
        name: "Trading212"
    },
    {
        name: "Coinbase"
    }
]

export const BrokerTypeEnum = {
	Alpaca: "Alpaca",
	Freedom24: "Freedom",
	Tradier: "Tradier",
	Trading212: "Trading212",
    Coinbase: "Coinbase"
}