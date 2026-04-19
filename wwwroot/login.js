async function login() {

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    const response = await fetch("/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
    });

    const data = await response.json();

    if (data.success) {

        // 🔥 SAVE USER EMAIL
        localStorage.setItem("userEmail", email);

        window.location.href = "index.html";

    } else {
        alert("❌ Invalid login");
    }
}