import { useState } from 'react'
import axios from 'axios'

function Login() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [message, setMessage] = useState('')

    const cardStyle = {
        maxWidth: '420px',
        margin: '40px auto',
        backgroundColor: '#ffffff',
        borderRadius: '12px',
        padding: '24px',
        boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)'
    }

    const inputStyle = {
        width: '100%',
        boxSizing: 'border-box',
        padding: '10px 12px',
        border: '1px solid #d1d5db',
        borderRadius: '8px'
    }

    const buttonStyle = {
        width: '100%',
        backgroundColor: '#2563eb',
        color: '#ffffff',
        border: 'none',
        borderRadius: '8px',
        padding: '10px 12px',
        cursor: 'pointer',
        fontWeight: 600
    }

    const handleLogin = async (event) => {
        event.preventDefault()

        try {
            const response = await axios.post(
                'https://localhost:7033/api/Auth/login',
                null,
                {
                    params: {
                        email: email,
                        password: password
                    }
                }
            )

            const token = response.data.token

            localStorage.setItem('token', token)

            setMessage('Login successful!')
            console.log('JWT Token:', token)
        } catch (error) {
            console.error(error)

            if (error.response?.status === 401) {
                setMessage('Invalid email or password.')
            } else {
                setMessage('Something went wrong. Please try again.')
            }
        }
    }

    return (
        <div style={cardStyle}>
            <h1 style={{ marginTop: 0 }}>Login</h1>

            <form onSubmit={handleLogin}>
                <div>
                    <label>Email</label>
                    <br />

                    <input
                        style={inputStyle}
                        type="email"
                        value={email}
                        onChange={(event) => setEmail(event.target.value)}
                        placeholder="Enter your email"
                    />
                </div>

                <br />

                <div>
                    <label>Password</label>
                    <br />

                    <input
                        style={inputStyle}
                        type="password"
                        value={password}
                        onChange={(event) => setPassword(event.target.value)}
                        placeholder="Enter your password"
                    />
                </div>

                <br />

                <button style={buttonStyle} type="submit">Login</button>
            </form>

            {message && (
                <p style={{ marginTop: '12px' }}>{message}</p>
            )}
        </div>
    )
}

export default Login