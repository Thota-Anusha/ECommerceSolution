import axios from 'axios'

const API_URL = 'https://localhost:7033/api/Order'

export const createOrder = async () => {
  const token = localStorage.getItem('token')

  const response = await axios.post(
    API_URL,
    null,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  )

  return response.data
}

export const getMyOrders = async () => {
  const token = localStorage.getItem('token')

  const response = await axios.get(
    API_URL,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  )

  return response.data
}