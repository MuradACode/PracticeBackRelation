let btns = document.querySelectorAll(".open-modal");
let modalLocation = document.querySelector(".product-details-modal");

btns.forEach(x => {
    x.addEventListener("click", function (e) {
        e.preventDefault();
        let prdId = x.getAttribute("data-id");
        let url = `http://localhost:9996/home/getproduct/${prdId}`;

        console.log(url);

        fetch(url)
            .then(response => response.text())
            .then(data => {
                    modalLocation.innerHTML = data;
                })
            })
    })
})