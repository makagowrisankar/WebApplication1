const BASE_URL = "https://universal-solver.onrender.com";

// 🔍 SEARCH
async function findServices() {
    const service = document.getElementById("search").value;

    const response = await fetch(`${BASE_URL}/providers?service=${service}`);
    const data = await response.json();

    const container = document.getElementById("services");
    container.innerHTML = "";

    data.forEach(p => {
        const card = document.createElement("div");

        card.innerHTML = `
            <h3>${p.Name}</h3>
            <p>${p.Service}</p>
            <button onclick="bookService('${p.Name}','${p.Service}')">Book</button>
        `;

        container.appendChild(card);
    });
}

// 📦 BOOK
async function bookService(name, service) {
    const response = await fetch(`${BASE_URL}/book`, {
        method: "POST"
    });

    const data = await response.json();

    if (data.success) {
        alert("Booking Success ✅");
    }
}