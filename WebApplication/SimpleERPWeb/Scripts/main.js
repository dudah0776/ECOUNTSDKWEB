function JsonDateParse(dateString) {
    var timestamp = parseInt(dateString.substr(6)); // 타임스탬프 값 추출
    var date = new Date(timestamp); // 타임스탬프를 사용하여 Date 객체 생성
    // 원하는 형식으로 날짜 포맷팅
    var formattedDate =
        date.getFullYear() + "-" + (date.getMonth() + 1).toString().padStart(2, "0") +
        "-" + date.getDate().toString().padStart(2, "0");
    return formattedDate;
}

function DateParse(dateString) {
    var dateParts = dateString.split(" ")[0].split("-");
    var year = dateParts[0];
    var month = dateParts[1];
    var day = dateParts[2];
    var formattedDate = year + "-" + month + "-" + day;
    return formattedDate;
}