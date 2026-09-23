import { useEffect, useState } from 'react'
import { getMyOrders } from '../services/orderService'

function Orders() {
    const [orders, setOrders] = useState([])
    const [message, setMessage] = useState('Loading orders...')

    const pageStyle = {
        backgroundColor: '#ffffff',
        borderRadius: '14px',
        padding: '20px',
        boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)'
    }

    const orderCardStyle = {
        border: '1px solid #e5e7eb',
        borderRadius: '10px',
        backgroundColor: '#f9fafb',
        padding: '14px',
        marginBottom: '12px'
    }

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const data = await getMyOrders()

                setOrders(data)
                setMessage('')
            } catch (error) {
                console.error(error)

                if (error.response?.status === 401) {
                    setMessage('Please login first.')
                } else {
                    setMessage('Unable to load orders.')
                }
            }
        }

        fetchOrders()
    }, [])

    return (
        <div style={pageStyle}>
            <h1 style={{ marginTop: 0 }}>My Orders</h1>

            {message && <p style={{ color: '#1d4ed8' }}>{message}</p>}

            {orders.map((order) => (
                <div
                    className="order-card"
                    key={order.orderId}
                    style={orderCardStyle}
                >
                    <h2>Order #{order.orderId}</h2>

                    <p>
                        Date: {new Date(order.orderDate).toLocaleString()}
                    </p>

                    <p>
                        Total Amount: ₹{order.totalAmount}
                    </p>

                    <p>
                        Payment Status:{' '}
                        <strong>{order.status}</strong>
                    </p>
                </div>
            ))}

            {orders.length === 0 && !message && (
                <p>You don't have any orders yet.</p>
            )}
        </div>
    )
}

export default Orders