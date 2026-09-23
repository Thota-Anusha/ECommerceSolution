import axios from 'axios'

const API_URL = 'https://localhost:7033/api/Cart'

export const addToCart = async (productId, quantity) => {
  const token = localStorage.getItem('token')

  const response = await axios.post(
    API_URL,
    null,
    {
      params: {
        productId: productId,
        quantity: quantity
      },
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  )

  return response.data
}

export const getCart = async () => {
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

export const updateCartItem = async (productId, quantity) => {
  const token = localStorage.getItem('token')

  const response = await axios.put(
    API_URL,
    null,
    {
      params: {
        productId: productId,
        quantity: quantity
      },
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  )

  return response.data
}

export const removeFromCart = async (productId) => {
  const token = localStorage.getItem('token')

  const response = await axios.delete(
    `${API_URL}/${productId}`,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  )

  return response.data
}