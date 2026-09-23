import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getCart, removeFromCart } from '../services/cartService'
import { getProductById } from '../services/productService'

function Cart() {
    const navigate = useNavigate()

    const [cartItems, setCartItems] = useState([])
    const [message, setMessage] = useState('Loading cart...')

    const pageStyle = {
        backgroundColor: '#ffffff',
        borderRadius: '14px',
        padding: '20px',
        boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)'
    }

    const itemStyle = {
        border: '1px solid #e5e7eb',
        borderRadius: '10px',
        padding: '14px',
        marginBottom: '12px',
        backgroundColor: '#f9fafb'
    }

    const buttonStyle = {
        backgroundColor: '#dc2626',
        color: '#ffffff',
        border: 'none',
        borderRadius: '8px',
        padding: '8px 12px',
        cursor: 'pointer',
        fontWeight: 600
    }

    const fetchCart = async () => {
        try {
            setMessage('Loading cart...')

            const cart = await getCart()

            const itemsWithProducts = await Promise.all(
                cart.cartItems.map(async (item) => {
                    const product = await getProductById(item.productId)

                    return {
                        ...item,
                        product
                    }
                })
            )

            setCartItems(itemsWithProducts)
            setMessage('')
        } catch (error) {
            console.error(error)

            if (error.response?.status === 401) {
                setMessage('Please login first.')
            } else if (error.response?.status === 404) {
                setCartItems([])
                setMessage('')
            } else {
                setMessage('Unable to load cart.')
            }
        }
    }

    useEffect(() => {
        // API data is loaded here and then stored in React state.
        // This is intentional for this data-fetching effect.
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchCart()
    }, [])

    const handleRemove = async (productId) => {
        try {
            setMessage('Removing product...')

            await removeFromCart(productId)

            await fetchCart()
        } catch (error) {
            console.error(error)
            setMessage('Unable to remove product.')
        }
    }

    const handleCheckout = () => {
        navigate('/checkout')
    }

    const total = cartItems.reduce(
        (sum, item) =>
            sum + item.product.price * item.quantity,
        0
    )

    return (
        <div className="cart-page" style={pageStyle}>

            <h1>My Cart</h1>

            {message && (
                <p style={{ color: '#1d4ed8' }}>{message}</p>
            )}

            {cartItems.length === 0 && !message && (
                <div className="empty-cart">
                    <h2>Your cart is empty</h2>

                    <p>
                        Add some products to your cart to continue.
                    </p>

                    <button
                        style={{
                            backgroundColor: '#2563eb',
                            color: '#ffffff',
                            border: 'none',
                            borderRadius: '8px',
                            padding: '8px 12px',
                            cursor: 'pointer',
                            fontWeight: 600
                        }}
                        onClick={() => navigate('/products')}
                    >
                        Continue Shopping
                    </button>
                </div>
            )}

            {cartItems.map((item) => (
                <div
                    className="cart-item"
                    key={item.cartItemId}
                    style={itemStyle}
                >
                    <h2>{item.product.name}</h2>

                    <p>
                        Price: ₹{item.product.price}
                    </p>

                    <p>
                        Quantity: {item.quantity}
                    </p>

                    <p>
                        Item Total: ₹
                        {item.product.price * item.quantity}
                    </p>

                    <button
                        style={buttonStyle}
                        onClick={() => handleRemove(item.productId)}
                    >
                        Remove
                    </button>
                </div>
            ))}

            {cartItems.length > 0 && (
                <div className="cart-total">

                    <h2>
                        Cart Total: ₹{total}
                    </h2>

                    <button
                        style={{
                            backgroundColor: '#059669',
                            color: '#ffffff',
                            border: 'none',
                            borderRadius: '8px',
                            padding: '10px 14px',
                            cursor: 'pointer',
                            fontWeight: 600
                        }}
                        onClick={handleCheckout}
                    >
                        Proceed to Checkout
                    </button>

                </div>
            )}

        </div>
    )
}

export default Cart