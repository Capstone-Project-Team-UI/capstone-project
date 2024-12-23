# Base URL for the API
$baseUrl = "http://localhost:8090/users"

# 1. Test GET endpoint: Get all users
Write-Host "Testing GET /users (Fetch All Users)..."
$response = Invoke-RestMethod -Uri "$baseUrl" -Method Get
Write-Host "Response: $($response | ConvertTo-Json -Depth 3)"
Write-Host ""

# 2. Test POST endpoint: Add a new user
Write-Host "Testing POST /users (Add User)..."
$newUser = @{
    userID = "user2"
    organization = "Company B"
    serialNumber = "Device456"
    uniqueID = "uniqueID456"
    emailAddress = "support@companyb.com"
}
$response = Invoke-RestMethod -Uri "$baseUrl" -Method Post -Body ($newUser | ConvertTo-Json) -ContentType "application/json"
Write-Host "Response: $($response)"
Write-Host ""

# 3. Test PUT endpoint: Update an existing user
Write-Host "Testing PUT /users/{userID} (Update User)..."
$updatedUser = @{
    organization = "Company C Updated"
    serialNumber = "Device789"
    uniqueID = "uniqueID789Updated"
    emailAddress = "updated_support@companyc.com"
}
$userIDToUpdate = "user3"
$response = Invoke-RestMethod -Uri "$baseUrl/$userIDToUpdate" -Method Put -Body ($updatedUser | ConvertTo-Json) -ContentType "application/json"
Write-Host "Response: $($response)"
Write-Host ""

# Optional: Test for invalid data (e.g., invalid userID in PUT request)
Write-Host "Testing PUT /users/{invalid_userID} (Update Non-existent User)..."
$updatedUserInvalid = @{
    organization = "Non-existent Company"
    serialNumber = "Device999"
    uniqueID = "uniqueID999"
    emailAddress = "nonexistent@company.com"
}
$invalidUserID = "invalid_user_id"
$response = Invoke-RestMethod -Uri "$baseUrl/$invalidUserID" -Method Put -Body ($updatedUserInvalid | ConvertTo-Json) -ContentType "application/json" -ErrorAction SilentlyContinue
Write-Host "Response: $($response)"
Write-Host ""

# Optional: Test POST request with missing data (invalid data)
Write-Host "Testing POST /users (Add User with Missing Data)..."
$invalidUser = @{
    userID = "user4"
    # Missing organization, serialNumber, uniqueID, and emailAddress
}
$response = Invoke-RestMethod -Uri "$baseUrl" -Method Post -Body ($invalidUser | ConvertTo-Json) -ContentType "application/json" -ErrorAction SilentlyContinue
Write-Host "Response: $($response)"
Write-Host ""

Write-Host "All tests complete."
