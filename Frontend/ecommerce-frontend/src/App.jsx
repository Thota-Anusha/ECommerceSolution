import { BrowserRouter, Routes, Route } from 'react-router-dom'

import Navbar from './components/Navbar'
import Products from './pages/Products'
import Login from './pages/Login'
import Cart from './pages/Cart'
import Orders from './pages/Orders'
import Checkout from './pages/Checkout'
import AdminDashboard from './pages/AdminDashboard'
import AdminProducts from './pages/AdminProducts'
import AdminInventory from './pages/AdminInventory'

function App() {
  const appShellStyle = {
    minHeight: '100vh',
    backgroundColor: '#f5f7fb',
    color: '#1f2937',
    fontFamily: 'Segoe UI, Arial, sans-serif'
  }

  const contentStyle = {
    maxWidth: '1100px',
    margin: '0 auto',
    padding: '20px'
  }

  return (
    <BrowserRouter>
      <div style={appShellStyle}>
        <Navbar />

        <main style={contentStyle}>
          <Routes>
            <Route path="/" element={<Products />} />
            <Route path="/products" element={<Products />} />
            <Route path="/login" element={<Login />} />
            <Route path="/cart" element={<Cart />} />
            <Route path="/orders" element={<Orders />} />
            <Route path="/checkout" element={<Checkout />} />
            <Route
              path="/admin"
              element={<AdminDashboard />}
            />
            <Route
              path="/admin/products"
              element={<AdminProducts />}
            />
            <Route
              path="/admin/inventory"
              element={<AdminInventory />}
            />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  )
}

export default App