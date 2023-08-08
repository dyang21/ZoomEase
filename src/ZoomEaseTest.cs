using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using zoomcs;
namespace ZoomEaseTest; // Replace with the namespace of your main project

[TestClass]
public class ProgramTests
{
    // ------------ Test 1: Should pass all test ------------
    [TestMethod]
    public async Task Test1()
    {
        var refToken = "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjU4OTk1MzNiLTZhYjAtNDNlYi1hNjQxLTUyMGQ0MzMxODM4ZiJ9.eyJ2ZXIiOjksImF1aWQiOiJiZmEwMDEwNTQxZWJmMGJmYTdhNGVmYTcxM2ZmZjI0NCIsImNvZGUiOiI1dFI3YU5iWmlsazV6VDFWNGNIVG5HYWtTb0NtQldwQWciLCJpc3MiOiJ6bTpjaWQ6WEZPVExUcWVSMXlyeDJaV2RNZHp3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6InM0MnpTVTBjU2lhWGdNay1lUjVXNWciLCJuYmYiOjE2ODkyNzQ0NTcsImV4cCI6MTY5NzA1MDQ1NywiaWF0IjoxNjg5Mjc0NDU3LCJhaWQiOiJSTnZoZkJIZlI4S2V5cXBGQUNKR0FRIn0.i4eBgzmpFD6h4xMNeZPgJAH6Tcvm3jP-9uJQHeQcwVd470sXzFCuIX1ARWgwPVKKCX3OhEh2f1NVGWTZWodqhQ";
        var cID = "XFOTLTqeR1yrx2ZWdMdzw";
        var cSecret = "79scsCkvrCvNOQyZ3Gb8jRUKypdJ3Uk5";

        // Arrange: Set up any input values or initial state.
        var testToken = await zoomcs.Program.RefreshAccessTokenAsync(refToken, cID, cSecret);
        string testTopic = "Test Meeting";
        string testPass = "Passwo";
        string testStart = "2024-08-10T12:00:00Z";
        int testDuration = 30;

        // Act: Call the method you're testing.
        var result = await zoomcs.Program.PostMeetings(testToken, testTopic, testPass, testStart, testDuration);

        // Assert: Check if the result is as expected.
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Item1); // Checking if join URL is not null
        Assert.IsNotNull(result.Item2); // Checking if start URL is not null
    }

    // ------------ Test 2: Pass invalid refToken, should fail w/ proper exception ------------
    [TestMethod]
    public async Task Test2()
    {
        var refToken = "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjU4OTk1MzNiLTZhYjAtNDNlYi1hNjQxLTUyMGQ0MzMxODM4ZiJ9.eyJ2ZXIiOjksImF1aWQiwNTQxZWJmMGJmYTdhNGVmYTcxM2ZmZjI0NCIsImNvZGUiOiI1dFI3YU5iWmlsazV6VDFWNGNIVG5HYWtTb0NtQldwQWciLCJpc3MiOiJ6bTpjaWQ6WEZPVExUcWVSMXlyeDJaV2RNZHp3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6InM0MnpTVTBjU2lhWGdNay1lUjVXNWciLCJuYmYiOjE2ODkyNzQ0NTcsImV4cCI6MTY5NzA1MDQ1NywiaWF0IjoxNjg5Mjc0NDU3LCJhaWQiOiJSTnZoZkJIZlI4S2V5cXBGQUNKR0FRIn0.i4eBgzmpFD6h4xMNeZPgJAH6Tcvm3jP-9uJQHeQcwVd470sXzFCuIX1ARWgwPVKKCX3OhEh2f1NVGWTZWodqhQ";
        var cID = "XFOTLTqeR1yrx2ZWdMdzw";
        var cSecret = "79scsCkvrCvNOQyZ3Gb8jRUKypdJ3Uk5";

        // Arrange: Set up any input values or initial state.
        var testToken = await zoomcs.Program.RefreshAccessTokenAsync(refToken, cID, cSecret);
        string testTopic = "Test Meeting";
        string testPass = "Passwo";
        string testStart = "2024-08-10T12:00:00Z";
        int testDuration = 30;

        // Act: Call the method you're testing.
        var result = await zoomcs.Program.PostMeetings(testToken, testTopic, testPass, testStart, testDuration);

        // Assert: Check if the result is as expected.
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Item1); // Checking if join URL is not null
        Assert.IsNotNull(result.Item2); // Checking if start URL is not null
    }

    // ------------ Test 3: Pass a password that has more than 10 characters, should fail (it will throw the wrong exception but that is because we throw an exception before even calling the method, so expected throw from testing is "Bad request...") ------------
    [TestMethod]
    public async Task Test3()
    {
        var refToken = "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjU4OTk1MzNiLTZhYjAtNDNlYi1hNjQxLTUyMGQ0MzMxODM4ZiJ9.eyJ2ZXIiOjksImF1aWQiOiJiZmEwMDEwNTQxZWJmMGJmYTdhNGVmYTcxM2ZmZjI0NCIsImNvZGUiOiI1dFI3YU5iWmlsazV6VDFWNGNIVG5HYWtTb0NtQldwQWciLCJpc3MiOiJ6bTpjaWQ6WEZPVExUcWVSMXlyeDJaV2RNZHp3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6InM0MnpTVTBjU2lhWGdNay1lUjVXNWciLCJuYmYiOjE2ODkyNzQ0NTcsImV4cCI6MTY5NzA1MDQ1NywiaWF0IjoxNjg5Mjc0NDU3LCJhaWQiOiJSTnZoZkJIZlI4S2V5cXBGQUNKR0FRIn0.i4eBgzmpFD6h4xMNeZPgJAH6Tcvm3jP-9uJQHeQcwVd470sXzFCuIX1ARWgwPVKKCX3OhEh2f1NVGWTZWodqhQ";
        var cID = "XFOTLTqeR1yrx2ZWdMdzw";
        var cSecret = "79scsCkvrCvNOQyZ3Gb8jRUKypdJ3Uk5";

        // Arrange: Set up any input values or initial state.
        var testToken = await zoomcs.Program.RefreshAccessTokenAsync(refToken, cID, cSecret);
        string testTopic = "Test Meeting";
        string testPass = "PasswordStreet";
        string testStart = "2024-08-10T12:00:00Z";
        int testDuration = 30;

        // Act: Call the method you're testing.
        var result = await zoomcs.Program.PostMeetings(testToken, testTopic, testPass, testStart, testDuration);

        // Assert: Check if the result is as expected.
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Item1); // Checking if join URL is not null
        Assert.IsNotNull(result.Item2); // Checking if start URL is not null
    }

    // ------------ Test 4: Pass a date that is in the past, should fail w/ proper exception ------------
    [TestMethod]
    public async Task Test4()
    {
        var refToken = "eyJzdiI6IjAwMDAwMSIsImFsZyI6IkhTNTEyIiwidiI6IjIuMCIsImtpZCI6IjU4OTk1MzNiLTZhYjAtNDNlYi1hNjQxLTUyMGQ0MzMxODM4ZiJ9.eyJ2ZXIiOjksImF1aWQiOiJiZmEwMDEwNTQxZWJmMGJmYTdhNGVmYTcxM2ZmZjI0NCIsImNvZGUiOiI1dFI3YU5iWmlsazV6VDFWNGNIVG5HYWtTb0NtQldwQWciLCJpc3MiOiJ6bTpjaWQ6WEZPVExUcWVSMXlyeDJaV2RNZHp3IiwiZ25vIjowLCJ0eXBlIjoxLCJ0aWQiOjAsImF1ZCI6Imh0dHBzOi8vb2F1dGguem9vbS51cyIsInVpZCI6InM0MnpTVTBjU2lhWGdNay1lUjVXNWciLCJuYmYiOjE2ODkyNzQ0NTcsImV4cCI6MTY5NzA1MDQ1NywiaWF0IjoxNjg5Mjc0NDU3LCJhaWQiOiJSTnZoZkJIZlI4S2V5cXBGQUNKR0FRIn0.i4eBgzmpFD6h4xMNeZPgJAH6Tcvm3jP-9uJQHeQcwVd470sXzFCuIX1ARWgwPVKKCX3OhEh2f1NVGWTZWodqhQ";
        var cID = "XFOTLTqeR1yrx2ZWdMdzw";
        var cSecret = "79scsCkvrCvNOQyZ3Gb8jRUKypdJ3Uk5";

        // Arrange: Set up any input values or initial state.
        var testToken = await zoomcs.Program.RefreshAccessTokenAsync(refToken, cID, cSecret);
        string testTopic = "Test Meeting";
        string testPass = "123456";
        string testStart = "2022-08-10T12:00:00Z";
        int testDuration = 30;

        // Act: Call the method you're testing.
        var result = await zoomcs.Program.PostMeetings(testToken, testTopic, testPass, testStart, testDuration);

        // Assert: Check if the result is as expected.
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Item1); // Checking if join URL is not null
        Assert.IsNotNull(result.Item2); // Checking if start URL is not null
    }
}
