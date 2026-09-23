import axios from 'axios'

const API_URL = 'https://localhost:7033/api/Product'

export const getProductById = async (productId) => {
  const token = localStorage.getItem('token')

  const response = await axios.get(
    `${API_URL}/${productId}`,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  )

  return response.data
}