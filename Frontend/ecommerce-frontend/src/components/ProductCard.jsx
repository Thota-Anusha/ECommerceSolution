import { useState } from 'react'
import { addToCart } from '../services/cartService'

function ProductCard({ product }) {
  const [message, setMessage] = useState('')

  const cardStyle = {
    backgroundColor: '#ffffff',
    border: '1px solid #e5e7eb',
    borderRadius: '12px',
    padding: '16px',
    width: '260px',
    boxShadow: '0 2px 8px rgba(15, 23, 42, 0.08)'
  }

  const buttonStyle = {
    backgroundColor: '#2563eb',
    color: '#ffffff',
    border: 'none',
    borderRadius: '8px',
    padding: '8px 12px',
    cursor: 'pointer',
    fontWeight: 600
  }

  const messageStyle = {
    marginTop: '10px',
    color: '#0f766e',
    fontSize: '14px'
  }

  const handleAddToCart = async () => {
    try {
      await addToCart(product.productId, 1)

      setMessage('Added to cart!')
    } catch (error) {
      console.error(error)

      if (error.response?.status === 401) {
        setMessage('Please login first.')
      } else {
        setMessage('Unable to add product to cart.')
      }
    }
  }

  return (
    <div className="product-card" style={cardStyle}>
      <h2 style={{ marginTop: 0 }}>{product.name}</h2>

      <p>Price: ₹{product.price}</p>

      <button style={buttonStyle} onClick={handleAddToCart}>
        Add to Cart
      </button>

      {message && <p style={messageStyle}>{message}</p>}
    </div>
  )
}

export default ProductCard