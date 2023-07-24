export function timeSince(date: Date): string {
    // Get the difference between 2 dates in seconds
    const seconds = (new Date(Date.now()).valueOf() - new Date(date).valueOf()) / 1000;

    let interval: number;

    interval = seconds / 31536000;
    if (interval > 1) {
        return Math.floor(interval) + " năm trước";
    }

    interval = seconds / 2630000;
    if (interval > 1) {
        return Math.floor(interval) + " tháng trước";
    }

    interval = seconds / 86400;
    if (interval > 1) {
        return Math.floor(interval) + " ngày trước";
    }

    interval = seconds / 3600;
    if (interval > 1) {
        return Math.floor(interval) + " giờ trước";
    }

    interval = seconds / 60;
    if (interval > 1) {
        return Math.floor(interval) + " phút trước";
    }

    return Math.floor(seconds) + " giây trước";

}