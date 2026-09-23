import { Link, useLocation } from 'react-router-dom'

function Navbar() {
  const location = useLocation()

  const navStyle = {
    backgroundColor: '#111827',
    padding: '14px 20px',
    display: 'flex',
    justifyContent: 'center',
    gap: '12px',
    flexWrap: 'wrap',
    boxShadow: '0 2px 8px rgba(0, 0, 0, 0.15)'
  }

  const getLinkStyle = (path) => ({
    color: '#ffffff',
    textDecoration: 'none',
    padding: '8px 12px',
    borderRadius: '8px',
    backgroundColor:
      location.pathname === path ? '#2563eb' : 'transparent',
    fontWeight: 600,
    fontSize: '14px'
  })

  return (
    <nav style={navStyle}>
      <Link style={getLinkStyle('/products')} to="/products">Products</Link>
      <Link style={getLinkStyle('/cart')} to="/cart">Cart</Link>
      <Link style={getLinkStyle('/orders')} to="/orders">My Orders</Link>
      <Link style={getLinkStyle('/checkout')} to="/checkout">Checkout</Link>
      <Link style={getLinkStyle('/admin')} to="/admin">Admin</Link>
      <Link style={getLinkStyle('/login')} to="/login">Login</Link>
    </nav>
  )
}

export default Navbar