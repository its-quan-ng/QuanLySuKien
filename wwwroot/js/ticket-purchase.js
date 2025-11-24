// ===================================
// TICKET PURCHASE FLOW
// ===================================

let selectedTickets = [];
let orderData = {
    tickets: [],
    attendee: {},
    total: 0
};

// Open Ticket Modal
function openTicketModal(eventId) {
    const modal = new bootstrap.Modal(document.getElementById('ticketPurchaseModal'));
    modal.show();
    resetTicketFlow();
}

window.openTicketModal = openTicketModal;

// Reset Flow
function resetTicketFlow() {
    selectedTickets = [];
    orderData = { tickets: [], attendee: {}, total: 0 };

    // Reset all quantities
    document.querySelectorAll('.qty-input').forEach(input => {
        input.value = 0;
    });

    // Go to step 1
    goToStep(1);
    updateStep1Total();
}

// Update Quantity
function updateQuantity(ticketId, change) {
    const input = document.getElementById('qty-' + ticketId);
    if (!input) return;

    const currentQty = parseInt(input.value) || 0;
    const max = parseInt(input.max);
    const newQty = Math.max(0, Math.min(max, currentQty + change));

    input.value = newQty;

    // Update selected tickets array
    const ticketSelector = input.closest('.ticket-type-selector');
    const ticketPrice = parseFloat(ticketSelector.dataset.ticketPrice);
    const ticketName = ticketSelector.dataset.ticketName;

    // Remove existing entry
    selectedTickets = selectedTickets.filter(t => t.id !== ticketId);

    // Add if quantity > 0
    if (newQty > 0) {
        selectedTickets.push({
            id: ticketId,
            name: ticketName,
            price: ticketPrice,
            quantity: newQty
        });
        ticketSelector.classList.add('selected');
    } else {
        ticketSelector.classList.remove('selected');
    }

    updateStep1Total();
}

window.updateQuantity = updateQuantity;

// Update Step 1 Total
function updateStep1Total() {
    const total = selectedTickets.reduce((sum, ticket) => {
        return sum + (ticket.price * ticket.quantity);
    }, 0);

    const totalElement = document.getElementById('step1Total');
    if (totalElement) {
        totalElement.textContent = total.toLocaleString('vi-VN') + ' VNĐ';
    }

    orderData.total = total;
}

// Navigate Between Steps
function goToStep(stepNumber) {
    // Validation
    if (stepNumber === 2) {
        if (selectedTickets.length === 0) {
            alert('Vui lòng chọn ít nhất một loại vé!');
            return;
        }
        generateAttendeeForm();
    }

    if (stepNumber === 3) {
        if (!validateAttendeeForm()) {
            return;
        }
        generateOrderSummary();
    }

    // Hide all steps
    document.querySelectorAll('.ticket-step').forEach(step => {
        step.style.display = 'none';
    });

    // Show selected step
    const selectedStep = document.getElementById('step' + stepNumber);
    if (selectedStep) {
        selectedStep.style.display = 'block';
    }

    // Update totals
    if (stepNumber === 2) {
        document.getElementById('step2Total').textContent =
            orderData.total.toLocaleString('vi-VN') + ' VNĐ';
    }
}

window.goToStep = goToStep;

// Generate Attendee Form
function generateAttendeeForm() {
    const container = document.getElementById('attendeeFormsContainer');
    if (!container) return;

    const totalTickets = selectedTickets.reduce((sum, t) => sum + t.quantity, 0);

    let formsHtml = '';
    let ticketIndex = 1;

    selectedTickets.forEach(ticket => {
        for (let i = 0; i < ticket.quantity; i++) {
            formsHtml += `
                <div class="attendee-form">
                    <div class="attendee-form-title">Vé #${ticketIndex} - ${ticket.name}</div>

                    <div class="form-group">
                        <label>Họ và Tên <span class="text-danger">*</span></label>
                        <input type="text" class="form-control attendee-name"
                               placeholder="Nhập họ và tên" required
                               data-ticket-index="${ticketIndex}">
                    </div>

                    <div class="form-group">
                        <label>Email <span class="text-danger">*</span></label>
                        <input type="email" class="form-control attendee-email"
                               placeholder="example@email.com" required
                               data-ticket-index="${ticketIndex}">
                    </div>

                    <div class="form-group">
                        <label>Số Điện Thoại <span class="text-danger">*</span></label>
                        <input type="tel" class="form-control attendee-phone"
                               placeholder="0912345678" required
                               data-ticket-index="${ticketIndex}">
                    </div>
                </div>
            `;
            ticketIndex++;
        }
    });

    container.innerHTML = formsHtml;
}

// Validate Attendee Form
function validateAttendeeForm() {
    const acceptTerms = document.getElementById('acceptTerms');
    if (!acceptTerms.checked) {
        alert('Vui lòng đồng ý với điều khoản dịch vụ!');
        return false;
    }

    const names = document.querySelectorAll('.attendee-name');
    const emails = document.querySelectorAll('.attendee-email');
    const phones = document.querySelectorAll('.attendee-phone');

    let isValid = true;

    names.forEach((input, index) => {
        if (!input.value.trim()) {
            input.style.borderColor = 'red';
            isValid = false;
        } else {
            input.style.borderColor = '';
        }
    });

    emails.forEach(input => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(input.value)) {
            input.style.borderColor = 'red';
            isValid = false;
        } else {
            input.style.borderColor = '';
        }
    });

    phones.forEach(input => {
        if (!input.value.trim() || input.value.length < 10) {
            input.style.borderColor = 'red';
            isValid = false;
        } else {
            input.style.borderColor = '';
        }
    });

    if (!isValid) {
        alert('Vui lòng điền đầy đủ thông tin hợp lệ!');
        return false;
    }

    // Save attendee data
    orderData.attendee = {
        name: names[0].value,
        email: emails[0].value,
        phone: phones[0].value
    };

    return true;
}

// Generate Order Summary
function generateOrderSummary() {
    // Ticket summary
    let ticketSummaryHtml = '';
    selectedTickets.forEach(ticket => {
        const subtotal = ticket.price * ticket.quantity;
        ticketSummaryHtml += `
            <div class="summary-item">
                <span class="label">${ticket.name} x ${ticket.quantity}</span>
                <span class="value">${subtotal.toLocaleString('vi-VN')} VNĐ</span>
            </div>
        `;
    });
    document.getElementById('ticketSummaryList').innerHTML = ticketSummaryHtml;

    // Attendee summary
    const attendeeSummaryHtml = `
        <div class="attendee-summary-item">
            <strong>Họ và Tên:</strong>
            <p>${orderData.attendee.name}</p>
        </div>
        <div class="attendee-summary-item">
            <strong>Email:</strong>
            <p>${orderData.attendee.email}</p>
        </div>
        <div class="attendee-summary-item">
            <strong>Số Điện Thoại:</strong>
            <p>${orderData.attendee.phone}</p>
        </div>
    `;
    document.getElementById('attendeeSummary').innerHTML = attendeeSummaryHtml;

    // Calculate totals
    const subtotal = orderData.total;
    const serviceFee = Math.round(subtotal * 0.05); // 5% service fee
    const finalTotal = subtotal + serviceFee;

    document.getElementById('subtotal').textContent = subtotal.toLocaleString('vi-VN') + ' VNĐ';
    document.getElementById('serviceFee').textContent = serviceFee.toLocaleString('vi-VN') + ' VNĐ';
    document.getElementById('finalTotal').textContent = finalTotal.toLocaleString('vi-VN') + ' VNĐ';

    orderData.serviceFee = serviceFee;
    orderData.finalTotal = finalTotal;
}

// Submit Order
async function submitOrder() {
    // Show loading state
    const payBtn = document.querySelector('.btn-pay-now');
    const originalText = payBtn.innerHTML;
    payBtn.disabled = true;
    payBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Đang xử lý...';

    // Prepare data for server
    const orderPayload = {
        suKienId: parseInt(window.location.pathname.split('/').pop()),
        tickets: selectedTickets.map(t => ({
            loaiVeId: parseInt(t.id),
            soLuong: t.quantity,
            giaVe: t.price
        })),
        tenKhachHang: orderData.attendee.name,
        email: orderData.attendee.email,
        soDienThoai: orderData.attendee.phone
    };

    try {
        // Get CSRF token
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        // POST to server
        const response = await fetch('/DonHangs/Purchase', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token || ''
            },
            body: JSON.stringify(orderPayload)
        });

        const result = await response.json();

        if (response.ok && result.success) {
            // Success - redirect to confirmation page
            window.location.href = '/DonHangs/Confirmation?orderIds=' + result.orderIds.join(',');
        } else {
            // Error
            alert('Có lỗi xảy ra: ' + (result.message || 'Vui lòng thử lại!'));
            payBtn.disabled = false;
            payBtn.innerHTML = originalText;
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Không thể kết nối đến server. Vui lòng thử lại!');
        payBtn.disabled = false;
        payBtn.innerHTML = originalText;
    }
}

window.submitOrder = submitOrder;
