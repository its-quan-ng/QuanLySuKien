// Admin Dashboard Charts

function initDashboardCharts(revenueData, ordersData) {
    // Revenue Chart
    const revenueLabels = revenueData.map(d => `${d.month}/${d.year}`);
    const revenueValues = revenueData.map(d => d.revenue);

    new Chart(document.getElementById('revenueChart'), {
        type: 'line',
        data: {
            labels: revenueLabels,
            datasets: [{
                label: 'Doanh Thu (VNĐ)',
                data: revenueValues,
                borderColor: '#10b981',
                backgroundColor: 'rgba(16, 185, 129, 0.1)',
                tension: 0.4,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString() + ' đ';
                        }
                    }
                }
            }
        }
    });

    // Orders by Status Chart
    const statusLabels = ordersData.map(d => {
        if (d.status === 'ChoDuyet') return 'Chờ Duyệt';
        if (d.status === 'DaXacNhan') return 'Đã Xác Nhận';
        if (d.status === 'DaHuy') return 'Đã Hủy';
        return d.status;
    });
    const statusCounts = ordersData.map(d => d.count);

    new Chart(document.getElementById('ordersChart'), {
        type: 'doughnut',
        data: {
            labels: statusLabels,
            datasets: [{
                data: statusCounts,
                backgroundColor: ['#fbbf24', '#10b981', '#ef4444'],
                borderWidth: 2,
                borderColor: '#fff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { position: 'bottom' }
            }
        }
    });
}
