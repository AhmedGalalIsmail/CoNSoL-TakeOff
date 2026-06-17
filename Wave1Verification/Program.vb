Option Strict On

Imports System.Collections.Generic
Imports System.Drawing
Imports Domain.Common
Imports Domain.Entities
Imports Domain.Utilities
Imports Domain.Validation

Module Program
	Private _passed As Integer
	Private _failed As Integer

	Sub Main()
		RunTest("Coordinate round trip", AddressOf CoordinateRoundTrip)
		RunTest("Selection mode resolution", AddressOf SelectionModeResolution)
		RunTest("Window selection", AddressOf WindowSelection)
		RunTest("Crossing selection", AddressOf CrossingSelection)
		RunTest("Canvas layout validation", AddressOf CanvasLayoutValidationPasses)
		RunTest("Canvas layout validation rejects bad scale", AddressOf CanvasLayoutValidationRejectsBadScale)
		RunTest("Canvas layout validation rejects bad unit", AddressOf CanvasLayoutValidationRejectsBadUnit)

		Console.WriteLine($"Passed: {_passed}, Failed: {_failed}")
		If _failed > 0 Then
			Environment.ExitCode = 1
		End If
	End Sub

	Private Sub RunTest(name As String, test As Action)
		Try
			test()
			_passed += 1
			Console.WriteLine($"PASS {name}")
		Catch ex As Exception
			_failed += 1
			Console.WriteLine($"FAIL {name}: {ex.Message}")
		End Try
	End Sub

	Private Sub CoordinateRoundTrip()
		Dim logical = CoordinateConverter.ToLogical(New PointF(24.0F, 36.0F), 3.0F)
		AssertNear(8.0F, logical.X)
		AssertNear(12.0F, logical.Y)

		Dim physical = CoordinateConverter.ToPhysical(logical, 3.0F)
		AssertNear(24.0F, physical.X)
		AssertNear(36.0F, physical.Y)
	End Sub

	Private Sub SelectionModeResolution()
		AssertEqual(SelectionMode.Window, SelectionRules.ResolveMode(New PointF(10.0F, 10.0F), New PointF(50.0F, 25.0F)))
		AssertEqual(SelectionMode.Crossing, SelectionRules.ResolveMode(New PointF(50.0F, 10.0F), New PointF(10.0F, 25.0F)))
	End Sub

	Private Sub WindowSelection()
		Dim selection = New RectangleF(0.0F, 0.0F, 100.0F, 100.0F)
		Dim inside = New RectangleF(20.0F, 20.0F, 10.0F, 10.0F)
		Dim overlapRect = New RectangleF(90.0F, 90.0F, 20.0F, 20.0F)

		If Not SelectionRules.ShouldSelect(inside, selection, SelectionMode.Window) Then
			Throw New Exception("Expected fully enclosed object to be selected in window mode.")
		End If

		If SelectionRules.ShouldSelect(overlapRect, selection, SelectionMode.Window) Then
			Throw New Exception("Expected partially overlapping object to be rejected in window mode.")
		End If
	End Sub

	Private Sub CrossingSelection()
		Dim selection = New RectangleF(0.0F, 0.0F, 100.0F, 100.0F)
		Dim overlapRect = New RectangleF(90.0F, 90.0F, 20.0F, 20.0F)

		If Not SelectionRules.ShouldSelect(overlapRect, selection, SelectionMode.Crossing) Then
			Throw New Exception("Expected partially overlapping object to be selected in crossing mode.")
		End If
	End Sub

	Private Sub CanvasLayoutValidationPasses()
		Dim layout As New CanvasLayout With {
			.ScaleFactor = 2.0R,
			.Unit = "meter"
		}

		CanvasLayoutValidation.Validate(layout)
	End Sub

	Private Sub CanvasLayoutValidationRejectsBadScale()
		Dim layout As New CanvasLayout With {
			.ScaleFactor = 0.0R,
			.Unit = "meter"
		}

		Dim thrown = False
		Try
			CanvasLayoutValidation.Validate(layout)
		Catch ex As ValidationException
			thrown = True
		End Try

		If Not thrown Then
			Throw New Exception("Expected validation to reject zero scale factor.")
		End If
	End Sub

	Private Sub CanvasLayoutValidationRejectsBadUnit()
		Dim layout As New CanvasLayout With {
			.ScaleFactor = 1.0R,
			.Unit = "parsec"
		}

		Dim thrown = False
		Try
			CanvasLayoutValidation.Validate(layout)
		Catch ex As ValidationException
			thrown = True
		End Try

		If Not thrown Then
			Throw New Exception("Expected validation to reject unsupported unit.")
		End If
	End Sub

	Private Sub AssertEqual(expected As SelectionMode, actual As SelectionMode)
		If Not EqualityComparer(Of SelectionMode).Default.Equals(expected, actual) Then
			Throw New Exception($"Expected {expected}, got {actual}.")
		End If
	End Sub

	Private Sub AssertNear(expected As Single, actual As Single)
		If Math.Abs(expected - actual) > 0.001F Then
			Throw New Exception($"Expected {expected}, got {actual}.")
		End If
	End Sub
End Module
