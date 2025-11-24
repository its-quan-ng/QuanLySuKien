# EVENTIFY DESIGN SYSTEM - WIREFRAME ANALYSIS
**Date**: 2025-11-23
**Source**: Figma Eventify Wireframe
**Status**: MASTER REFERENCE - DO NOT DEVIATE FROM THIS

---

## 🎨 COLOR PALETTE (Adapting to EventHub)

### Original Eventify Colors:
- **Primary Dark Navy**: `#3C3D5A` (navbar, footer, dark buttons)
- **Accent Yellow**: `#FFC107` or `#FFD700` (logo, badges, CTAs)
- **White**: `#FFFFFF` (backgrounds, cards)
- **Light Gray**: `#F5F5F5` (backgrounds)
- **Text Gray**: `#666666` (secondary text)

### EventHub Adaptation:
- **Primary**: `#8B1538` (Burgundy) - replaces dark navy in buttons
- **Secondary**: `#1B3A5F` (Navy Blue) - replaces dark navy in navbar/footer
- **Accent**: `#FFC107` (Keep Yellow) - for category badges and highlights
- **Neutral Beige**: `#F5F1E8` (backgrounds)
- **White**: `#FFFFFF` (cards, backgrounds)

---

## 📐 LAYOUT STRUCTURE

### 1. NAVIGATION BAR (Image #6, #7)
```
Structure:
- Height: ~70px
- Background: Dark Navy (#1B3A5F for EventHub)
- Sticky: YES
- Container: Full width with padding

Left Section:
- Logo: Ticket icon + "Eventify" text (yellow)
  → For EventHub: Keep ticket icon + "EventHub" in white/gradient

Center Section:
- Links: Home | Events | About | Contact
- Font weight: 500
- Hover: Underline effect

Right Section:
- "Create Event" button (white text)
- Icons with labels:
  * Tickets icon + "Tickets"
  * Star icon + "Interested"
  * Profile icon + "Profile" (with dropdown arrow)
- All in white color
```

### 2. FOOTER (Image #2, #3, #4, #8)
```
Structure:
- Background: Dark Navy (#1B3A5F)
- Color: White/Gray text
- 5 Columns with equal width
- Padding: 60px 0

Columns:
1. Company Info
   - About Us
   - Contact Us
   - Careers
   - FAQs
   - Terms of Service
   - Privacy Policy

2. Help
   - Account Support
   - Listing Events
   - Event Ticketing
   - Ticket Purchase Terms & Conditions

3. Categories
   - Concerts & Gigs
   - Festivals & Lifestyle
   - Business & Networking
   - Food & Drinks
   - Performing Arts
   - Sports & Outdoors
   - Exhibitions
   - Workshops, Conferences & Classes

4. Follow Us
   - Facebook
   - Instagram
   - Twitter
   - Youtube

5. Download The App
   - Google Play button (with icon)
   - App Store button (with icon)

Bottom:
- Copyright: "©2023 Eventify. All rights reserved."
```

---

## 🎴 EVENT CARD DESIGN (Image #5, #8, #9)

### Card Structure:
```
Dimensions:
- Width: Responsive (3 cols desktop, 2 cols tablet, 1 col mobile)
- Aspect Ratio: ~16:9 for image
- Border Radius: 8px
- Shadow: Subtle on hover (elevation)
- Background: White

Components (Top to Bottom):

1. IMAGE SECTION:
   - Full width, height: ~200px
   - Object-fit: cover
   - Border-radius: 8px 8px 0 0

   Overlays on Image:
   a) DATE BADGE (Top Left):
      - Position: Absolute, top: 12px, left: 12px
      - Background: White
      - Padding: 8px 12px
      - Border-radius: 4px
      - Text:
        * Month (uppercase, red/burgundy, font-size: 10px, font-weight: 700)
        * Day (black, font-size: 18px, font-weight: 700)

   b) FAVORITE ICON (Top Right):
      - Position: Absolute, top: 12px, right: 12px
      - Icon: Star outline
      - Background: White circle
      - Size: 32px
      - Hover: Filled star

   c) CATEGORY BADGE (Bottom Left, on image):
      - Position: Absolute, bottom: 12px, left: 12px
      - Background: Yellow (#FFC107)
      - Color: Black
      - Padding: 4px 12px
      - Border-radius: 4px
      - Font-size: 11px
      - Font-weight: 600

2. CONTENT SECTION:
   - Padding: 16px

   a) EVENT TITLE:
      - Font-size: 16px
      - Font-weight: 600
      - Color: Dark gray/black
      - Max-lines: 2
      - Line-height: 1.4
      - Margin-bottom: 8px

   b) VENUE:
      - Font-size: 13px
      - Color: Gray (#666)
      - Icon: Location pin (before text)
      - Margin-bottom: 4px

   c) TIME:
      - Font-size: 13px
      - Color: Gray (#666)
      - Format: "00:00 AM - 00:00 PM"
      - Margin-bottom: 8px

   d) FOOTER ROW:
      - Display: Flex, justify-between

      Left: PRICE
        - Icon: Ticket (green)
        - Text: "INR 499" (EventHub: VND format)
        - Font-weight: 600
        - Color: Green (#00A651) → EventHub: Burgundy

      Right: INTERESTED COUNT
        - Icon: Star (filled)
        - Text: "76 Interested"
        - Font-size: 12px
        - Color: Gray

Hover Effect:
- Transform: translateY(-4px)
- Shadow: 0 8px 20px rgba(0,0,0,0.12)
- Transition: 0.3s ease
```

---

## 🏠 HOMEPAGE LAYOUT (Image #5, #8)

### 1. HERO SECTION:
```
- Background: Light gray (#F5F5F5) or promotional image
- Padding: 60px 0
- Center aligned

Title:
- "Explore a world of events. Find what excites you!"
- Font-size: 36px
- Font-weight: 700
- Color: White (if on image) or Dark Navy

Search Bar:
- Width: 600px (max)
- Height: 56px
- Background: White
- Border-radius: 28px
- Shadow: 0 2px 8px rgba(0,0,0,0.1)
- Display: Flex

  Left Input (Search):
  - Icon: Search
  - Placeholder: "Delhi" (city name)
  - Border-right: 1px solid gray
  - Flex: 1

  Right Dropdown (Location):
  - Icon: Location pin
  - Text: "Mumbai"
  - Dropdown arrow
  - Width: 200px
```

### 2. EXPLORE CATEGORIES:
```
- Margin: 40px 0
- Title: "Explore Categories"
- Font-size: 24px
- Font-weight: 600

Category Items:
- Display: Flex (horizontal scroll on mobile)
- Each item:
  * Circle: 80px diameter
  * Background: Light gray
  * Icon/Image inside
  * Label below: "Category 1"
  * Font-size: 14px
  * Hover: Scale 1.05
```

### 3. EVENT SECTIONS:
```
Multiple sections with same structure:
- "Popular Events in Mumbai"
- "Discover Best of Online Events"
- "Trending Events around the World"

Each Section:
- Title + Filter Pills (All, Today, Tomorrow, This Weekend, More)
- Grid: 3 columns (desktop), 2 columns (tablet)
- Cards: 6 items per section
- "Label" button at bottom (load more)
- Some sections have "Featured" badge on cards
```

### 4. CTA BANNER:
```
- Background: Light gray
- Padding: 60px
- Center aligned

Text:
- "Create an event with Eventify"
- "Are you an organizer, event manager or a brand..."
- Font-size: 24px

Button:
- Background: Yellow (#FFC107)
- Text: "Create Event" with arrow
- Padding: 14px 32px
- Border-radius: 6px
- Font-weight: 600
```

### 5. NEWSLETTER:
```
- Background: Yellow (#FFC107)
- Padding: 40px 0
- Display: Flex (left text, right form)

Text:
- "Subscribe to our Newsletter"
- Description below

Form:
- Input: White background, placeholder
- Button: Dark navy "Subscribe"
- Display: Inline flex
```

---

## 📋 EVENTS LISTING PAGE (Image #9)

### Layout:
```
Structure: Sidebar + Main Content (2-column layout)

LEFT SIDEBAR (Width: 250px):
==================
Header: "Filters"

Sections (each collapsible):

1. PRICE:
   - [ ] Free
   - [ ] Paid

2. DATE:
   - [ ] Today
   - [ ] Tomorrow
   - [ ] This Week
   - [ ] This Weekend
   - [ ] Pick a Date (datepicker)
   - "More" link

3. CATEGORY:
   - [ ] Adventure Travel
   - [ ] Art Exhibitions
   - [ ] Auctions & Fundraisers
   - [ ] Beer Festivals
   - [ ] Benefit Concerts
   - "More" link

4. FORMAT:
   - [ ] Community Engagement
   - [ ] Concerts & Performances
   - [ ] Conferences
   - [ ] Experiential Events
   - [ ] Festivals & Fairs
   - "More" link

Styling:
- Checkboxes: Custom styled
- Font-size: 14px
- Spacing: 12px between items
- Section spacing: 24px

RIGHT CONTENT (Flex: 1):
==================
Top Bar:
- "Sort by: Relevance" dropdown (right aligned)

Event Grid:
- 2 columns
- Same card design as homepage
- Infinite scroll or pagination
```

---

## 📄 EVENT DETAIL PAGE (Image #4)

### Layout:
```
Structure: Single column, max-width: 900px, centered

1. HERO IMAGE:
   - Full width
   - Height: 400px
   - Border-radius: 8px

2. HEADER SECTION:
   - Title (H1, font-size: 32px, font-weight: 700)
   - Icons: Star (favorite) + Share

3. DATE AND TIME:
   - Icon: Calendar
   - "Day, Date"
   - Icon: Clock
   - "Time"
   - "+ Add to Calendar" link (burgundy)

4. LOCATION:
   - Icon: Location pin
   - "Address"
   - Embedded Map (Google Maps)
     * Height: 250px
     * Border-radius: 8px
     * Red marker at location

5. TICKET INFORMATION:
   - Icon: Ticket
   - "Ticket Type: Price /ticket"

6. HOSTED BY:
   - Avatar (circular, 60px)
   - "Host Name"
   - Buttons: "Contact" + "+ Follow"

7. EVENT DESCRIPTION:
   - Font-size: 15px
   - Line-height: 1.7
   - Long form text

8. TAGS:
   - Pills/badges
   - Background: Light gray
   - Border-radius: 16px
   - Padding: 6px 16px

9. OTHER EVENTS YOU MAY LIKE:
   - Horizontal carousel
   - 3 cards visible
   - Left/right arrows
   - Same card design as listing

RIGHT SIDEBAR (Sticky):
==================
- "Buy Tickets" button
  * Background: Yellow (#FFC107)
  * Font-size: 16px
  * Font-weight: 600
  * Width: 100%
  * Padding: 14px
  * Border-radius: 6px
```

---

## 📝 EVENT CREATION FLOW (Image #2, #3, #7, #12)

### Progress Stepper:
```
Design:
- Horizontal line across page
- Circles at each step
- Steps: Edit → Banner → Ticketing → Review

Active Step:
- Circle: Filled (dark navy)
- Text: Bold, dark navy
- Line before: Solid dark navy

Inactive Step:
- Circle: Gray outline
- Text: Gray
- Line: Gray

Completed Step:
- Circle: Filled dark navy with checkmark
- Line: Solid dark navy
```

### Form Sections:

#### STEP 1: EDIT (Image #3)
```
Sections with clear headers:

1. Event Details:
   - Event Title * (required)
   - Event Category * (dropdown)

2. Date & Time:
   - Event Type: ( ) Single Event  ( ) Recurring Event
   - Session(s) *
   - Start Date * (datepicker)
   - Start Time * (timepicker)
   - End Time (timepicker)
   - Add session button (+)

3. Location:
   - "Where will your event take place?" * (dropdown)

4. Additional Information:
   - Event Description * (large textarea)
   - Placeholder: "Describe what's special..."

Bottom:
- "Save & Continue" button (dark navy, right aligned)
```

#### STEP 2: BANNER (Image #2)
```
Content:
- "Upload Image" title
- File input: "Choose File"
- Requirements text:
  * "Feature Image must be at least 1170 pixels wide by 504 pixels high."
  * "Valid file formats: JPG, GIF, PNG."

Bottom:
- "Go back to Edit Event" (text link, left)
- "Save & Continue" button (dark navy, right)
```

#### STEP 3: TICKETING (Image #12)
```
Section 1:
- "What type of event are you running?"
- Two large option cards:
  a) Ticketed Event (with ticket icon)
     "My event requires tickets for entry"
  b) Free Event (with FREE badge icon)
     "I'm running a free event"

Section 2:
- "What tickets are you selling?"
- Ticket Name input
- Ticket Price input (with currency symbol)
- Add ticket button (+)

Bottom:
- "Go back" (text link, left)
- "Save & Continue" button (dark navy, right)
```

#### STEP 4: REVIEW (Image #7)
```
Content:
- "Nearly there! Check everything's correct."
- Large preview card (bordered):
  * Event image
  * Event title
  * Date and Time section
  * Location with map
  * Ticket Information
  * Hosted by section
  * Event Description

Bottom:
- "Save For Later" button (yellow, left)
- "Publish Events" button (dark navy, right)
```

---

## 🎫 TICKET PURCHASE FLOW (Image #11)

### Modal Design:
```
Structure: 3-step modal with close (X) button

STEP 1: Select Tickets
====================
Header: "Select Tickets"

Content:
- Ticket Types list:
  * Green left border
  * Ticket name + price
  * Quantity selector (- 1 +)

Footer:
- "Qty: 1  Total: ₹200"
- "Proceed >" button (dark navy, full width)

STEP 2: Attendee Details
====================
Header: "← Attendee Details"

Content:
- Event Title + Date
- "Standard Ticket: Ticket #1"
- Form:
  * Full Name
  * Email
  * Phone (with country code dropdown)
- Checkbox: "I accept the Terms of Service..."

Footer:
- "Qty: 1  Total: ₹200"
- "Continue to Checkout >" button (gray, full width)

STEP 3: Order Summary
====================
Header: "← Order Summary"

Content:
- Blue box: "Ticket Type"
- Attendee's Name
- Email
- Price (blue badge, right)

Summary:
- Sub Total: ₹200.00
- Tax: ₹11.80
- Order Total: ₹211.80

Button:
- "🔒 Pay Now" (green, full width)
```

---

## 🎭 ACCOUNT SETTINGS (Image #1)

### Layout:
```
Structure: Left sidebar + Right content

LEFT SIDEBAR (Width: 200px):
- "Account Settings" header
- Menu items:
  * Account Info (active: white bg)
  * Change Email
  * Password

RIGHT CONTENT:
==================
Sections:

1. Profile Photo:
   - Circular avatar (120px)
   - Camera icon overlay (bottom right)
   - Upload button

2. Profile Information:
   - First Name input
   - Last Name input
   - Website input
   - Company input

3. Contact Details:
   - Helper text above
   - Phone Number input
   - Address input
   - City/Town input
   - Country dropdown
   - Postcode input

Bottom:
- "Save My Profile" button (dark navy)
```

---

## 🎯 KEY UI PATTERNS TO REMEMBER

### Buttons:
```
Primary (Dark Navy → Burgundy for EventHub):
- Background: #8B1538
- Color: White
- Padding: 12px 32px
- Border-radius: 6px
- Font-weight: 600
- Hover: Lighter shade

Secondary (Yellow):
- Background: #FFC107
- Color: Black
- Same padding/radius
- Hover: Darker shade

Text Link:
- Color: Burgundy
- Underline on hover
```

### Typography:
```
H1: 32px, 700
H2: 24px, 600
H3: 18px, 600
Body: 15px, 400
Small: 13px, 400
Caption: 11px, 600 (for badges)

Line Height: 1.5-1.7 for body text
```

### Spacing:
```
Section padding: 60px 0
Container max-width: 1200px
Grid gap: 24px
Card padding: 16px
Form input height: 44px
Button height: 44px
```

### Shadows:
```
Card: 0 2px 8px rgba(0,0,0,0.08)
Card hover: 0 8px 20px rgba(0,0,0,0.12)
Button hover: 0 4px 12px rgba(139,21,56,0.3)
```

### Border Radius:
```
Cards: 8px
Buttons: 6px
Input fields: 6px
Badges: 4px
Pills: 16px (full rounded)
Avatar: 50% (circular)
```

---

## 📱 RESPONSIVE BREAKPOINTS

```
Desktop: 1200px+
  - 3 column grid
  - Full sidebar filters

Tablet: 768px - 1199px
  - 2 column grid
  - Collapsible sidebar

Mobile: < 768px
  - 1 column grid
  - Bottom sheet filters
  - Hamburger menu
  - Stacked layout
```

---

## ✅ IMPLEMENTATION CHECKLIST

- [ ] Setup global CSS variables with EventHub colors
- [ ] Create sticky navbar component
- [ ] Build footer component (5 columns)
- [ ] Design event card component
- [ ] Implement homepage hero with search
- [ ] Create category circles section
- [ ] Build event grid with filters
- [ ] Develop event detail page layout
- [ ] Create multi-step form with progress stepper
- [ ] Implement ticket purchase modal
- [ ] Build account settings page
- [ ] Add hover effects and transitions
- [ ] Test responsive layout

---

## 🎨 EVENTIFY → EVENTHUB COLOR MAPPING

```css
/* Original Eventify → EventHub Adaptation */

/* Navbar & Footer Background */
#3C3D5A → #1B3A5F (Navy Blue)

/* Primary Buttons */
#3C3D5A → #8B1538 (Burgundy)

/* Accent/Highlight */
#FFC107 → #FFC107 (Keep Yellow - works with burgundy!)

/* Background */
#F5F5F5 → #F5F1E8 (Beige)

/* Success/Green */
#00A651 → #8B1538 (Use Burgundy instead)

/* Price color */
Green → Burgundy #8B1538
```

---

**NOTES:**
- This wireframe is GOSPEL - follow it exactly
- Only change colors as specified above
- Keep all spacing, typography, and layout exactly as designed
- Yellow accent (#FFC107) pairs beautifully with burgundy!
- When in doubt, refer back to this document
