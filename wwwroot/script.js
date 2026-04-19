const BASE_URL = "https://universal-solver.onrender.com";

// 🔍 SEARCH
async function findServices() {
    const service = document.getElementById("search").value;

    const response = await fetch(`${BASE_URL}/providers?service=${service}&lat=16.5062&lng=80.6480`);
    const data = await response.json();

    const container = document.getElementById("services");
    container.innerHTML = "";

    if (data.length === 0) {
        container.innerHTML = "<p>No service found</p>";
        return;
    }

    data.forEach(p => {
        const card = document.createElement("div");
        card.className = "card";

        const title = document.createElement("h3");
        title.innerText = p.name;

        const serviceText = document.createElement("p");
        serviceText.innerText = p.service;

        const button = document.createElement("button");
        button.innerText = "Book";

        button.addEventListener("click", function () {
            bookService(p.name, p.service);
        });

        card.appendChild(title);
        card.appendChild(serviceText);
        card.appendChild(button);

        container.appendChild(card);
    });
}

// 📦 BOOK FUNCTION
async function bookService(providerName, service) {
    try {
        const response = await fetch(`${BASE_URL}/book`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                userEmail: "admin@gmail.com",
                service: service,
                providerName: providerName
            })
        });

        const data = await response.json();

        if (data.success) {
            alert("🎉 Booking Successful!");
        } else {
            alert("❌ Booking Failed");
        }

    } catch (error) {
        console.error(error);
        alert("❌ Server error");
    }
}