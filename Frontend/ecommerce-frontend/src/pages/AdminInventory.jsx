
import { useEffect, useState } from "react";
const API_URL = "https://localhost:7033/api/Inventory";
const PRODUCT_API_URL = "https://localhost:7033/api/Product";

function AdminInventory() {
  const [inventory, setInventory] = useState([]);
  const [products, setProducts] = useState([]);
  const [productId, setProductId] = useState("");
  const [quantity, setQuantity] = useState("");
  const [editingId, setEditingId] = useState(null);
  const [error, setError] = useState("");

  const pageStyle = {
    backgroundColor: "#ffffff",
    borderRadius: "14px",
    padding: "24px",
    boxShadow: "0 4px 14px rgba(15, 23, 42, 0.08)",
  };

  const inputStyle = {
    width: "100%",
    maxWidth: "420px",
    padding: "10px 12px",
    border: "1px solid #d1d5db",
    borderRadius: "8px",
    boxSizing: "border-box",
  };

  const primaryButtonStyle = {
    backgroundColor: "#2563eb",
    color: "#ffffff",
    border: "none",
    borderRadius: "8px",
    padding: "8px 12px",
    cursor: "pointer",
    fontWeight: 600,
  };

  const dangerButtonStyle = {
    backgroundColor: "#dc2626",
    color: "#ffffff",
    border: "none",
    borderRadius: "8px",
    padding: "8px 12px",
    cursor: "pointer",
    fontWeight: 600,
  };

  const getErrorMessage = async (response, fallbackMessage) => {
    const contentType = response.headers.get("content-type") || "";

    if (contentType.includes("application/json")) {
      const payload = await response.json();

      if (typeof payload === "string" && payload.trim()) {
        return payload;
      }

      if (payload?.message) {
        return payload.message;
      }

      if (payload?.title) {
        return payload.title;
      }
    } else {
      const text = await response.text();

      if (text.trim()) {
        return text;
      }
    }

    return fallbackMessage;
  };

  // Get all inventory
  const fetchInventory = async () => {
    try {
      const token = localStorage.getItem("token");

      const response = await fetch(API_URL, {
        headers: {
          Authorization: `Bearer ${ token }`,
        },
      });

      if (!response.ok) {
        throw new Error("Failed to load inventory");
      }

      const data = await response.json();
      setInventory(data);
    } catch (err) {
      setError(err.message);
    }
  };

  const fetchProducts = async () => {
    try {
      const token = localStorage.getItem("token");

      const response = await fetch(PRODUCT_API_URL, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error("Failed to load products");
      }

      const data = await response.json();
      setProducts(data);
    } catch (err) {
      setError(err.message);
    }
  };

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    fetchInventory();
    // eslint-disable-next-line react-hooks/set-state-in-effect
    fetchProducts();
  }, []);

  // Add or Update
  const handleSubmit = async (e) => {
    e.preventDefault();

    setError("");

    if (!productId || !quantity) {
      setError("Please enter Product ID and Quantity");
      return;
    }

    const parsedProductId = Number(productId);
    const parsedQuantity = Number(quantity);

    if (!Number.isInteger(parsedProductId) || parsedProductId <= 0) {
      setError("Product ID must be a positive number");
      return;
    }

    if (!Number.isInteger(parsedQuantity) || parsedQuantity < 0) {
      setError("Quantity must be zero or greater");
      return;
    }

    const token = localStorage.getItem("token");

    const data = {
      inventoryId: editingId || 0,
      productId: parsedProductId,
      quantity: parsedQuantity,
    };

    try {
      let response;

      const existingInventory = inventory.find(
        (item) => item.productId === parsedProductId
      );

      if (editingId || existingInventory) {
        const targetInventoryId = editingId || existingInventory.inventoryId;

        // UPDATE
        response = await fetch(`${API_URL}/${targetInventoryId}`, {
method: "PUT",
    headers: {
    "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
          },
body: JSON.stringify({
  inventoryId: targetInventoryId,
  productId: parsedProductId,
  quantity: parsedQuantity,
}),
        });
      } else {
    // ADD
    response = await fetch(API_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(data),
    });
}

if (!response.ok) {
    if (response.status === 401) {
        throw new Error("You are not authorized. Please login.");
    }

    if (response.status === 403) {
        throw new Error("Only Admin users can manage inventory.");
    }

    const backendMessage = await getErrorMessage(
      response,
      "Operation failed"
    );

    throw new Error(backendMessage);
}

// Clear form
setProductId("");
setQuantity("");
setEditingId(null);

// Reload inventory
await fetchInventory();
    } catch (err) {
    setError(err.message);
}
  };

// Edit
const handleEdit = (item) => {
    setEditingId(item.inventoryId);
    setProductId(String(item.productId));
    setQuantity(String(item.quantity));
};

// Delete
const handleDelete = async (id) => {
    const confirmDelete = window.confirm(
        "Are you sure you want to delete this inventory?"
    );

    if (!confirmDelete) {
        return;
    }

    try {
        const token = localStorage.getItem("token");

        const response = await fetch(`${API_URL}/${id}`, {
            method: "DELETE",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });

        if (!response.ok) {
            if (response.status === 401) {
                throw new Error("You are not authorized. Please login.");
            }

            if (response.status === 403) {
                throw new Error("Only Admin users can delete inventory.");
            }

            const backendMessage = await getErrorMessage(
                response,
                "Delete failed"
            );

            throw new Error(backendMessage);
        }

        fetchInventory();
    } catch (err) {
        setError(err.message);
    }
};

// Cancel edit
const handleCancel = () => {
    setEditingId(null);
    setProductId("");
    setQuantity("");
    setError("");
};

return (
    <div style={pageStyle}>
        <h2>Admin Inventory Management</h2>

        {error && (
            <p style={{ color: "#b91c1c", backgroundColor: "#fee2e2", padding: "10px", borderRadius: "8px" }}>
                {error}
            </p>
        )}

        {/* FORM */}

        <form onSubmit={handleSubmit}>

            <div>
                <label>Product ID</label>
                <br />

                <select
                    style={inputStyle}
                    value={productId}
                    onChange={(e) => setProductId(e.target.value)}
                >
                    <option value="">Select Product</option>
                    {products.map((product) => (
                        <option
                            key={product.productId}
                            value={product.productId}
                        >
                            {product.productId} - {product.name}
                        </option>
                    ))}
                </select>
            </div>

            <br />

            <div>
                <label>Quantity</label>
                <br />

                <input
                    style={inputStyle}
                    type="number"
                    value={quantity}
                    onChange={(e) => setQuantity(e.target.value)}
                    placeholder="Enter Quantity"
                />
            </div>

            <br />

            <button style={primaryButtonStyle} type="submit">
                {editingId ? "Update Inventory" : "Add Inventory"}
            </button>

            {editingId && (
                <button
                    type="button"
                    onClick={handleCancel}
                    style={{ ...primaryButtonStyle, marginLeft: "10px", backgroundColor: "#6b7280" }}
                >
                    Cancel
                </button>
            )}

        </form>

        <hr />

        {/* INVENTORY TABLE */}

        <h3>Inventory List</h3>

        {inventory.length === 0 ? (
            <p>No inventory records found.</p>
        ) : (
            <table style={{ width: "100%", borderCollapse: "collapse", border: "1px solid #d1d5db" }} cellPadding="10">
                <thead>
                    <tr style={{ backgroundColor: "#f3f4f6" }}>
                        <th>Inventory ID</th>
                        <th>Product ID</th>
                        <th>Quantity</th>
                        <th>Actions</th>
                    </tr>
                </thead>

                <tbody>
                    {inventory.map((item) => (
                        <tr key={item.inventoryId}>

                            <td>{item.inventoryId}</td>

                            <td>{item.productId}</td>

                            <td>{item.quantity}</td>

                            <td>
                                <button
                                    style={primaryButtonStyle}
                                    onClick={() => handleEdit(item)}
                                >
                                    Edit
                                </button>

                                <button
                                    onClick={() =>
                                        handleDelete(item.inventoryId)
                                    }
                                    style={{ ...dangerButtonStyle, marginLeft: "10px" }}
                                >
                                    Delete
                                </button>
                            </td>

                        </tr>
                    ))}
                </tbody>
            </table>
        )}
    </div>
);
}

export default AdminInventory;
