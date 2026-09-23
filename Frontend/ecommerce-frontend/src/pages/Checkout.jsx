import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { createOrder } from '../services/orderService'
import { createPayment } from '../services/paymentService'

function Checkout() {
    const navigate = useNavigate()

    const [paymentMethod, setPaymentMethod] = useState('UPI')
    const [message, setMessage] = useState('')
    const [processing, setProcessing] = useState(false)
    const [success, setSuccess] = useState(false)

    const pageStyle = {
        backgroundColor: '#ffffff',
        borderRadius: '14px',
        padding: '20px',
        boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)',
        maxWidth: '640px'
    }

    const optionStyle = {
        padding: '10px 12px',
        border: '1px solid #e5e7eb',
        borderRadius: '8px',
        marginBottom: '10px',
        backgroundColor: '#f9fafb'
    }

    const payButtonStyle = {
        width: '100%',
        backgroundColor: '#2563eb',
        color: '#ffffff',
        border: 'none',
        borderRadius: '8px',
        padding: '10px 12px',
        cursor: 'pointer',
        fontWeight: 600
    }

    const handlePayment = async () => {
        try {
            setProcessing(true)
            setSuccess(false)
            setMessage('Processing payment...')

            // Simulate payment processing
            await new Promise((resolve) =>
                setTimeout(resolve, 1500)
            )

            // Step 1: Create the order
            const order = await createOrder()

            // Step 2: Create payment for the order
            const payment = await createPayment(order.orderId)

            console.log('Payment created:', payment)

            // Payment was successful
            setSuccess(true)

            setMessage(
                `Payment successful! Order #${order.orderId} has been placed.`
            )

            // Go to orders page after 2 seconds
            setTimeout(() => {
                navigate('/orders')
            }, 2000)

        } catch (error) {
            console.error('Payment error:', error)

            setSuccess(false)

            if (error.response?.status === 401) {
                setMessage('Please login first.')
            } else if (error.response?.status === 400) {
                setMessage(
                    error.response?.data ||
                    'Unable to place the order.'
                )
            } else {
                setMessage(
                    'Payment failed. Please try again.'
                )
            }

        } finally {
            setProcessing(false)
        }
    }

    return (
        <div className="checkout-page" style={pageStyle}>

            <h1 style={{ marginTop: 0 }}>Checkout</h1>

            <div className="checkout-card">

                <h2>Payment Method</h2>

                <div className="payment-option" style={optionStyle}>
                    <label>
                        <input
                            type="radio"
                            value="UPI"
                            checked={paymentMethod === 'UPI'}
                            onChange={(event) =>
                                setPaymentMethod(event.target.value)
                            }
                        />
                        UPI
                    </label>
                </div>

                <div className="payment-option" style={optionStyle}>
                    <label>
                        <input
                            type="radio"
                            value="Card"
                            checked={paymentMethod === 'Card'}
                            onChange={(event) =>
                                setPaymentMethod(event.target.value)
                            }
                        />
                        Credit / Debit Card
                    </label>
                </div>

                <div className="payment-option" style={optionStyle}>
                    <label>
                        <input
                            type="radio"
                            value="Cash on Delivery"
                            checked={paymentMethod === 'Cash on Delivery'}
                            onChange={(event) =>
                                setPaymentMethod(event.target.value)
                            }
                        />
                        Cash on Delivery
                    </label>
                </div>

                <div className="selected-payment">
                    <p>
                        Selected payment method:
                    </p>

                    <strong>{paymentMethod}</strong>
                </div>

                <button
                    className="pay-button"
                    style={payButtonStyle}
                    onClick={handlePayment}
                    disabled={processing}
                >
                    {processing
                        ? 'Processing Payment...'
                        : 'Pay & Place Order'}
                </button>

                {message && (
                    <p
                        style={{
                            marginTop: '12px',
                            color: success ? '#047857' : '#b91c1c'
                        }}
                        className={
                            success
                                ? 'success-message'
                                : 'checkout-message'
                        }
                    >
                        {message}
                    </p>
                )}

            </div>

        </div>
    )
}

export default Checkout