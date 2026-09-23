import { useEffect, useState } from 'react'
import axios from 'axios'

const API_URL = 'https://localhost:7033/api/Product'

function AdminProducts() {
    // ==========================================
    // STATE
    // ==========================================

    const [products, setProducts] = useState([])

    const [message, setMessage] = useState(
        'Loading products...'
    )

    const [loading, setLoading] = useState(false)

    // Add product
    const [showAddForm, setShowAddForm] = useState(false)

    const [newProduct, setNewProduct] = useState({
        name: '',
        description: '',
        price: '',
        categoryId: ''
    })

    // Edit product
    const [editingProduct, setEditingProduct] = useState(null)

    // Excel
    const [excelFile, setExcelFile] = useState(null)

    const pageStyle = {
        backgroundColor: '#ffffff',
        borderRadius: '14px',
        padding: '24px',
        boxShadow: '0 4px 14px rgba(15, 23, 42, 0.08)'
    }

    const sectionStyle = {
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
        padding: '16px',
        marginBottom: '16px',
        backgroundColor: '#f9fafb'
    }

    const formStyle = {
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
        padding: '16px',
        marginTop: '12px',
        marginBottom: '16px',
        backgroundColor: '#ffffff'
    }

    const inputStyle = {
        width: '100%',
        maxWidth: '420px',
        boxSizing: 'border-box',
        padding: '10px 12px',
        border: '1px solid #d1d5db',
        borderRadius: '8px'
    }

    const primaryButtonStyle = {
        backgroundColor: '#2563eb',
        color: '#ffffff',
        border: 'none',
        borderRadius: '8px',
        padding: '8px 12px',
        cursor: 'pointer',
        fontWeight: 600
    }

    const dangerButtonStyle = {
        backgroundColor: '#dc2626',
        color: '#ffffff',
        border: 'none',
        borderRadius: '8px',
        padding: '8px 12px',
        cursor: 'pointer',
        fontWeight: 600
    }

    // ==========================================
    // GET ALL PRODUCTS
    // ==========================================

    const fetchProducts = async () => {
        try {
            const token = localStorage.getItem('token')

            const response = await axios.get(
                API_URL,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            )

            setProducts(response.data)

            setMessage('')
        } catch (error) {
            console.error(
                'Get products error:',
                error
            )

            if (error.response?.status === 401) {
                setMessage(
                    'You are not authorized. Please login.'
                )
            } else if (error.response?.status === 403) {
                setMessage(
                    'Access denied. Please login as Admin.'
                )
            } else {
                setMessage(
                    'Unable to load products.'
                )
            }
        }
    }

    // Load products when page opens
    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchProducts()
    }, [])

    // ==========================================
    // ADD PRODUCT
    // ==========================================

    const handleAddProduct = async (event) => {
        event.preventDefault()

        try {
            setLoading(true)

            setMessage(
                'Adding product...'
            )

            const token =
                localStorage.getItem('token')

            const product = {
                name: newProduct.name,
                description:
                    newProduct.description,
                price: Number(
                    newProduct.price
                ),
                categoryId: Number(
                    newProduct.categoryId
                )
            }

            console.log(
                'Adding product:',
                product
            )

            const response =
                await axios.post(
                    API_URL,
                    product,
                    {
                        headers: {
                            Authorization:
                                `Bearer ${token}`,
                            'Content-Type':
                                'application/json'
                        }
                    }
                )

            console.log(
                'Product created:',
                response.data
            )

            setMessage(
                'Product added successfully!'
            )

            // Reset form
            setNewProduct({
                name: '',
                description: '',
                price: '',
                categoryId: ''
            })

            // Close form
            setShowAddForm(false)

            // Refresh products
            await fetchProducts()

        } catch (error) {
            console.error(
                'Add product error:',
                error
            )

            if (
                error.response?.status === 401
            ) {
                setMessage(
                    'You are not authorized. Please login.'
                )
            } else if (
                error.response?.status === 403
            ) {
                setMessage(
                    'Access denied. Please login as Admin.'
                )
            } else if (
                error.response?.status === 400
            ) {
                setMessage(
                    error.response?.data ||
                    'Invalid product details.'
                )
            } else {
                setMessage(
                    'Unable to add product.'
                )
            }
        } finally {
            setLoading(false)
        }
    }

    // ==========================================
    // DELETE PRODUCT
    // ==========================================

    const handleDelete = async (
        productId
    ) => {
        const confirmDelete =
            window.confirm(
                'Are you sure you want to delete this product?'
            )

        if (!confirmDelete) {
            return
        }

        try {
            setLoading(true)

            setMessage(
                'Deleting product...'
            )

            const token =
                localStorage.getItem('token')

            await axios.delete(
                `${API_URL}/${productId}`,
                {
                    headers: {
                        Authorization:
                            `Bearer ${token}`
                    }
                }
            )

            setMessage(
                'Product deleted successfully!'
            )

            await fetchProducts()

        } catch (error) {
            console.error(
                'Delete product error:',
                error
            )

            if (
                error.response?.status === 401
            ) {
                setMessage(
                    'You are not authorized. Please login.'
                )
            } else if (
                error.response?.status === 403
            ) {
                setMessage(
                    'Access denied. Please login as Admin.'
                )
            } else {
                setMessage(
                    'Unable to delete product.'
                )
            }
        } finally {
            setLoading(false)
        }
    }

    // ==========================================
    // START EDIT
    // ==========================================

    const handleEdit = (
        product
    ) => {
        setEditingProduct({
            productId:
                product.productId,

            name:
                product.name,

            description:
                product.description || '',

            price:
                product.price,

            categoryId:
                product.categoryId
        })

        setMessage('')
    }

    // ==========================================
    // UPDATE PRODUCT
    // ==========================================

    const handleUpdate = async (
        event
    ) => {
        event.preventDefault()

        try {
            setLoading(true)

            setMessage(
                'Updating product...'
            )

            const token =
                localStorage.getItem('token')

            // Create the exact Product
            // object expected by backend
            const product = {
                productId:
                    Number(
                        editingProduct.productId
                    ),

                name:
                    editingProduct.name,

                description:
                    editingProduct.description || '',

                price:
                    Number(
                        editingProduct.price
                    ),

                categoryId:
                    Number(
                        editingProduct.categoryId
                    )
            }

            console.log(
                'Updating product:',
                product
            )

            await axios.put(
                `${API_URL}/${product.productId}`,
                product,
                {
                    headers: {
                        Authorization:
                            `Bearer ${token}`,
                        'Content-Type':
                            'application/json'
                    }
                }
            )

            setMessage(
                'Product updated successfully!'
            )

            // Close edit form
            setEditingProduct(null)

            // Refresh product list
            await fetchProducts()

        } catch (error) {
            console.error(
                'Update product error:',
                error
            )

            console.error(
                'Response:',
                error.response?.data
            )

            if (
                error.response?.status === 401
            ) {
                setMessage(
                    'You are not authorized. Please login.'
                )
            } else if (
                error.response?.status === 403
            ) {
                setMessage(
                    'Access denied. Please login as Admin.'
                )
            } else if (
                error.response?.status === 400
            ) {
                setMessage(
                    error.response?.data ||
                    'Invalid product details.'
                )
            } else {
                setMessage(
                    'Unable to update product.'
                )
            }
        } finally {
            setLoading(false)
        }
    }

    // ==========================================
    // BULK EXCEL UPLOAD
    // ==========================================

    const handleBulkUpload = async (
        event
    ) => {
        event.preventDefault()

        if (!excelFile) {
            setMessage(
                'Please select an Excel file.'
            )

            return
        }

        if (
            !excelFile.name
                .toLowerCase()
                .endsWith('.xlsx')
        ) {
            setMessage(
                'Only .xlsx Excel files are allowed.'
            )

            return
        }

        try {
            setLoading(true)

            setMessage(
                'Uploading Excel file...'
            )

            const token =
                localStorage.getItem('token')

            const formData =
                new FormData()

            formData.append(
                'file',
                excelFile
            )

            const response =
                await axios.post(
                    `${API_URL}/bulk-upload`,
                    formData,
                    {
                        headers: {
                            Authorization:
                                `Bearer ${token}`
                        }
                    }
                )

            setMessage(
                response.data.message ||
                'Products uploaded successfully!'
            )

            setExcelFile(null)

            // Clear file input
            event.target.reset()

            // Refresh product list
            await fetchProducts()

        } catch (error) {
            console.error(
                'Excel upload error:',
                error
            )

            if (
                error.response?.status === 401
            ) {
                setMessage(
                    'You are not authorized. Please login.'
                )
            } else if (
                error.response?.status === 403
            ) {
                setMessage(
                    'Access denied. Please login as Admin.'
                )
            } else if (
                error.response?.status === 400
            ) {
                setMessage(
                    error.response?.data ||
                    'Invalid Excel file.'
                )
            } else {
                setMessage(
                    'Unable to upload Excel file.'
                )
            }
        } finally {
            setLoading(false)
        }
    }

    // ==========================================
    // RENDER
    // ==========================================

    return (
        <div className="admin-products" style={pageStyle}>

            <h1>
                Product Management
            </h1>

            {/* ======================================
          MESSAGE
          ====================================== */}

            {message && (
                <p style={{ backgroundColor: '#eff6ff', color: '#1d4ed8', padding: '10px', borderRadius: '8px' }}>
                    {message}
                </p>
            )}

            {/* ======================================
          ADD PRODUCT
          ====================================== */}

            <div className="admin-section" style={sectionStyle}>

                <button
                    style={{ ...primaryButtonStyle, marginBottom: '8px' }}
                    onClick={() =>
                        setShowAddForm(
                            !showAddForm
                        )
                    }
                    disabled={loading}
                >
                    {showAddForm
                        ? 'Cancel Add Product'
                        : 'Add Product'}
                </button>

                {showAddForm && (

                    <div className="admin-form" style={formStyle}>

                        <h2>
                            Add New Product
                        </h2>

                        <form
                            onSubmit={
                                handleAddProduct
                            }
                        >

                            {/* Product Name */}

                            <div>
                                <label>
                                    Product Name
                                </label>

                                <input
                                    style={inputStyle}
                                    type="text"
                                    value={
                                        newProduct.name
                                    }
                                    onChange={(
                                        event
                                    ) =>
                                        setNewProduct({
                                            ...newProduct,
                                            name:
                                                event.target.value
                                        })
                                    }
                                    placeholder="Enter product name"
                                    required
                                />
                            </div>

                            <br />

                            {/* Description */}

                            <div>
                                <label>
                                    Description
                                </label>

                                <input
                                    style={inputStyle}
                                    type="text"
                                    value={
                                        newProduct.description
                                    }
                                    onChange={(
                                        event
                                    ) =>
                                        setNewProduct({
                                            ...newProduct,
                                            description:
                                                event.target.value
                                        })
                                    }
                                    placeholder="Enter product description"
                                    required
                                />
                            </div>

                            <br />

                            {/* Price */}

                            <div>
                                <label>
                                    Price
                                </label>

                                <input
                                    style={inputStyle}
                                    type="number"
                                    value={
                                        newProduct.price
                                    }
                                    onChange={(
                                        event
                                    ) =>
                                        setNewProduct({
                                            ...newProduct,
                                            price:
                                                event.target.value
                                        })
                                    }
                                    placeholder="Enter price"
                                    min="0"
                                    step="0.01"
                                    required
                                />
                            </div>

                            <br />

                            {/* Category */}

                            <div>
                                <label>
                                    Category ID
                                </label>

                                <input
                                    style={inputStyle}
                                    type="number"
                                    value={
                                        newProduct.categoryId
                                    }
                                    onChange={(
                                        event
                                    ) =>
                                        setNewProduct({
                                            ...newProduct,
                                            categoryId:
                                                event.target.value
                                        })
                                    }
                                    placeholder="Enter category ID"
                                    min="1"
                                    required
                                />
                            </div>

                            <br />

                            <button
                                style={primaryButtonStyle}
                                type="submit"
                                disabled={loading}
                            >
                                {loading
                                    ? 'Saving...'
                                    : 'Save Product'}
                            </button>

                        </form>

                    </div>
                )}

            </div>

            {/* ======================================
          BULK EXCEL UPLOAD
          ====================================== */}

            <div className="admin-section" style={sectionStyle}>

                <h2>
                    Bulk Product Upload
                </h2>

                <p>
                    Upload an Excel (.xlsx)
                    file to add multiple
                    products.
                </p>

                <form
                    onSubmit={
                        handleBulkUpload
                    }
                >

                    <input
                        style={inputStyle}
                        type="file"
                        accept=".xlsx"
                        onChange={(
                            event
                        ) =>
                            setExcelFile(
                                event.target.files[0]
                            )
                        }
                    />

                    <br />
                    <br />

                    <button
                        style={primaryButtonStyle}
                        type="submit"
                        disabled={loading}
                    >
                        {loading
                            ? 'Uploading...'
                            : 'Upload Excel'}
                    </button>

                </form>

            </div>

            {/* ======================================
          EDIT PRODUCT
          ====================================== */}

            {editingProduct && (

                <div className="admin-form" style={formStyle}>

                    <h2>
                        Edit Product
                    </h2>

                    <form
                        onSubmit={
                            handleUpdate
                        }
                    >

                        {/* Product Name */}

                        <div>
                            <label>
                                Product Name
                            </label>

                            <input
                                style={inputStyle}
                                type="text"
                                value={
                                    editingProduct.name
                                }
                                onChange={(
                                    event
                                ) =>
                                    setEditingProduct({
                                        ...editingProduct,
                                        name:
                                            event.target.value
                                    })
                                }
                                required
                            />
                        </div>

                        <br />

                        {/* Description */}

                        <div>
                            <label>
                                Description
                            </label>

                            <input
                                style={inputStyle}
                                type="text"
                                value={
                                    editingProduct.description ||
                                    ''
                                }
                                onChange={(
                                    event
                                ) =>
                                    setEditingProduct({
                                        ...editingProduct,
                                        description:
                                            event.target.value
                                    })
                                }
                                placeholder="Enter description"
                                required
                            />
                        </div>

                        <br />

                        {/* Price */}

                        <div>
                            <label>
                                Price
                            </label>

                            <input
                                style={inputStyle}
                                type="number"
                                value={
                                    editingProduct.price
                                }
                                onChange={(
                                    event
                                ) =>
                                    setEditingProduct({
                                        ...editingProduct,
                                        price:
                                            event.target.value
                                    })
                                }
                                min="0"
                                step="0.01"
                                required
                            />
                        </div>

                        <br />

                        {/* Category ID */}

                        <div>
                            <label>
                                Category ID
                            </label>

                            <input
                                style={inputStyle}
                                type="number"
                                value={
                                    editingProduct.categoryId
                                }
                                onChange={(
                                    event
                                ) =>
                                    setEditingProduct({
                                        ...editingProduct,
                                        categoryId:
                                            event.target.value
                                    })
                                }
                                min="1"
                                required
                            />
                        </div>

                        <br />

                        <button
                            style={primaryButtonStyle}
                            type="submit"
                            disabled={loading}
                        >
                            {loading
                                ? 'Saving...'
                                : 'Save Changes'}
                        </button>

                        {' '}

                        <button
                            style={{ ...primaryButtonStyle, backgroundColor: '#6b7280', marginLeft: '8px' }}
                            type="button"
                            onClick={() =>
                                setEditingProduct(
                                    null
                                )
                            }
                            disabled={loading}
                        >
                            Cancel
                        </button>

                    </form>

                </div>
            )}

            {/* ======================================
          PRODUCT LIST
          ====================================== */}

            <div className="admin-product-list" style={{ marginTop: '20px' }}>

                <h2>
                    All Products
                </h2>

                {products.length === 0 &&
                    !message && (
                        <p>
                            No products found.
                        </p>
                    )}

                {products.map(
                    (product) => (

                        <div
                            className="admin-product-card"
                            style={sectionStyle}
                            key={
                                product.productId
                            }
                        >

                            <h2>
                                {product.name}
                            </h2>

                            <p>
                                <strong>
                                    Description:
                                </strong>{' '}
                                {product.description}
                            </p>

                            <p>
                                <strong>
                                    Price:
                                </strong>{' '}
                                ₹{product.price}
                            </p>

                            <p>
                                <strong>
                                    Category ID:
                                </strong>{' '}
                                {product.categoryId}
                            </p>

                            <p>
                                <strong>
                                    Product ID:
                                </strong>{' '}
                                {product.productId}
                            </p>

                            <button
                                style={primaryButtonStyle}
                                onClick={() =>
                                    handleEdit(
                                        product
                                    )
                                }
                                disabled={loading}
                            >
                                Edit
                            </button>

                            {' '}

                            <button
                                style={{ ...dangerButtonStyle, marginLeft: '8px' }}
                                onClick={() =>
                                    handleDelete(
                                        product.productId
                                    )
                                }
                                disabled={loading}
                            >
                                Delete
                            </button>

                        </div>

                    )
                )}

            </div>

        </div>
    )
}

export default AdminProducts