import { Link } from 'react-router-dom'

function AdminDashboard() {
    const pageStyle = {
        backgroundColor: '#ffffff',
        borderRadius: '14px',
        padding: '24px',
        boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)'
    }

    const menuStyle = {
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(240px, 1fr))',
        gap: '16px'
    }

    const cardStyle = {
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
        padding: '16px',
        backgroundColor: '#f9fafb'
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

    return (
        <div className="admin-dashboard" style={pageStyle}>

            <h1>Admin Dashboard</h1>

            <p>
                Welcome to the E-Commerce Admin Panel
            </p>

            <div className="admin-menu" style={menuStyle}>

                <div className="admin-card" style={cardStyle}>
                    <h2>Product Management</h2>

                    <p>
                        Add, edit, delete and bulk upload products.
                    </p>

                    <Link to="/admin/products">
                        <button style={buttonStyle}>Manage Products</button>
                    </Link>
                </div>

                <div className="admin-card" style={cardStyle}>
                    <h2>Inventory Management</h2>

                    <p>
                        View and manage product inventory.
                    </p>

                    <Link to="/admin/inventory">
                        <button style={buttonStyle}>Manage Inventory</button>
                    </Link>
                </div>

            </div>

        </div>
    )
}

export default AdminDashboard