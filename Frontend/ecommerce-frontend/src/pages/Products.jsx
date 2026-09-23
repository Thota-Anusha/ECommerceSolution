import { useEffect, useState } from 'react'
import axios from 'axios'
import ProductCard from '../components/ProductCard'

function Products() {
  const [products, setProducts] = useState([])
  const [message, setMessage] = useState('Loading products...')

  const pageStyle = {
    backgroundColor: '#ffffff',
    borderRadius: '14px',
    padding: '20px',
    boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)'
  }

  const gridStyle = {
    display: 'flex',
    flexWrap: 'wrap',
    gap: '16px'
  }

  const messageStyle = {
    backgroundColor: '#eff6ff',
    color: '#1d4ed8',
    borderRadius: '8px',
    padding: '10px 12px'
  }

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const token = localStorage.getItem('token')

        const response = await axios.get(
          'https://localhost:7033/api/Product',
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        )

        setProducts(response.data)
        setMessage('')
      } catch (error) {
        console.error(error)

        if (error.response?.status === 401) {
          setMessage('You are not authorized. Please login again.')
        } else {
          setMessage('Unable to load products.')
        }
      }
    }

    fetchProducts()
  }, [])

  return (
    <div style={pageStyle}>
      <h1 style={{ marginTop: 0 }}>Products</h1>

      {message && <p style={messageStyle}>{message}</p>}

      <div style={gridStyle}>
        {products.map((product) => (
          <ProductCard
            key={product.productId}
            product={product}
          />
        ))}
      </div>
    </div>
  )
}

export default Products