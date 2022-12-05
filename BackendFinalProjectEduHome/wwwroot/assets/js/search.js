

var searchInput = document.getElementById('search-courses');

searchInput.addEventListener('keyup', function () {
    var searchValue = searchInput.value;
  
    var productList = document.querySelector('#course-list');
   
    if (searchValue.length == 0) {
        productList.innerHTML = '';
    } else {
        fetch('/Course/Search?searchText=' + searchValue)
            .then((response) => response.text())
            .then((data) => {
                productList.innerHTML = data
            });
    }
});