import axios from 'axios'

const API_URL = 'https://localhost:7033/api/Payment'

export const createPayment = async (orderId) => {
    const token = localStorage.getItem('token')

    const response = await axios.post(
        `${API_URL}/${orderId}`,
        null,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    )

    return response.data
}

export const getPaymentByOrderId = async (orderId) => {
    const token = localStorage.getItem('token')

    const response = await axios.get(
        `${API_URL}/order/${orderId}`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    )

    return response.data
}